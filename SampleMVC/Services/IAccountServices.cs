using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyRESTServices.BLL.DTOs;

namespace SampleMVC.Services
{
    public interface IAccountServices
    {
        Task<UserDTO> Login(LoginDTO loginDTO);
    }
}