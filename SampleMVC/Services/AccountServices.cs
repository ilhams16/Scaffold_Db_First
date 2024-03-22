using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyRESTServices.BLL.DTOs;

namespace SampleMVC.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CategoryServices> _logger;

        public AccountServices(HttpClient client, IConfiguration configuration, ILogger<CategoryServices> logger)
        {
            _client = client;
            _configuration = configuration;
            _logger = logger;
        }
        private string GetBaseUrl()
        {
            return _configuration["BaseUrl"] + "/Accounts";
        }

        public async Task<UserWithTokenDTO> Login(LoginDTO loginDTO)
        {
            var json = JsonSerializer.Serialize(loginDTO);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{GetBaseUrl()}/Login", data);
            // _logger.LogInformation($"{GetBaseUrl()}/Login");
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve account");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserWithTokenDTO>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return user;
        }
    }
}