using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MyRESTServices.BLL.DTOs;
using MyRESTServices.BLL.Interfaces;
using MyRESTServices.Data.Interfaces;

namespace MyRESTServices.BLL
{
    public class UserBLL : IUserBLL
    {
        private readonly IUserData _userData;
        private readonly IMapper _mapper;
        //private readonly ILogger _logger;

        public UserBLL(IUserData userData, IMapper mapper)
        {
            _userData = userData;
            _mapper = mapper;
            //_logger = logger;
        }

        public Task<Task> ChangePassword(string username, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<Task> Delete(string username)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserDTO>> GetAllWithRoles()
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> GetByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> GetUserWithRoles(string username)
        {
            throw new NotImplementedException();
        }

        public Task<Task> Insert(UserCreateDTO entity)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDTO> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username is required");
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password is required");
            }
            try
            {
                var result = await _userData.Login(username, Helper.GetHash(password));
                //_logger.LogInformation(Helper.GetHash(password));
                if (result == null)
                {
                    throw new ArgumentException("Username or Password is wrong");
                }
                var userDto = _mapper.Map<UserDTO>(result);
                return userDto;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public Task<UserDTO> LoginMVC(LoginDTO loginDTO)
        {
            throw new NotImplementedException();
        }
    }
}