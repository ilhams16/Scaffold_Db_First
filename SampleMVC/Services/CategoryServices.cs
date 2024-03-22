using MyRESTServices.BLL.DTOs;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SampleMVC.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CategoryServices> _logger;

        public CategoryServices(HttpClient client, IConfiguration configuration, ILogger<CategoryServices> logger, IHttpContextAccessor context)
        {
            _client = client;
            _configuration = configuration;
            _logger = logger;
            _context = context;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _context.HttpContext.Session.GetString("userToken"));
        }

        private string GetBaseUrl()
        {
            return _configuration["BaseUrl"] + "/Categories";
        }

        public async Task<IEnumerable<CategoryDTO>> GetAll()
        {
            // _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _context.HttpContext.Session.GetString("userToken"));
            _logger.LogInformation(GetBaseUrl());
            var httpResponse = await _client.GetAsync(GetBaseUrl());

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve category");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var categories = JsonSerializer.Deserialize<IEnumerable<CategoryDTO>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return categories;
        }
        public async Task<IEnumerable<CategoryDTO>> GetAllWithPaging(int pageNumber, int pageSize, string? name)
        {
            //_logger.LogInformation(GetBaseUrl());
            //var httpResponse = await _client.GetAsync($"{GetBaseUrl()}/pageNumber={pageNumber}/pageSize={pageSize}/search={name}");
            // _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _context.HttpContext.Session.GetString("userToken"));
            var httpResponse = await _client.GetAsync(GetBaseUrl());
            if (!httpResponse.IsSuccessStatusCode){
                if ((int)httpResponse.StatusCode == 403){
                    throw new UnauthorizedAccessException("Unauthorized access");
                } else if ((int)httpResponse.StatusCode == 401) {
                    throw new Exception("Forbidden access");
                }
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var categories = JsonSerializer.Deserialize<IEnumerable<CategoryDTO>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            var pagingCategories = categories;
            if (name != null)
            {
                pagingCategories = pagingCategories.Where(c=>c.CategoryName.Contains(name));
            }
            pagingCategories = pagingCategories.OrderBy(c => c.CategoryName)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

            return pagingCategories;
        }


        public async Task<CategoryDTO> GetById(int id)
        {
            var httpResponse = await _client.GetAsync($"{GetBaseUrl()}/{id}");
            _logger.LogInformation($"{GetBaseUrl()}/{id}");
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve category");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var category = JsonSerializer.Deserialize<CategoryDTO>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return category;
        }

        public async Task<int> GetCount(string name)
        {
            var httpResponse = await _client.GetAsync($"{GetBaseUrl()}/Count/{name}");
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve category");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var count = JsonSerializer.Deserialize<int>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return count;
        }

        //post
        public async Task<CategoryDTO> Insert(CategoryCreateDTO categoryCreateDTO)
        {
            var json = JsonSerializer.Serialize(categoryCreateDTO);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(GetBaseUrl(), data);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot insert category");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var category = JsonSerializer.Deserialize<CategoryDTO>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return category;
        }

        //put
        public async Task Update(int id, CategoryUpdateDTO categoryUpdateDTO)
        {
            var json = JsonSerializer.Serialize(categoryUpdateDTO);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync($"{GetBaseUrl()}/{id}", data);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot update category");
            }
        }

        //delete
        public async Task Delete(int id)
        {
            var httpResponse = await _client.DeleteAsync($"{GetBaseUrl()}/{id}");

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot delete category");
            }
        }
    }
}
