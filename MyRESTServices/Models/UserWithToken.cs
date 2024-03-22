using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MyRESTServices.BLL.DTOs;

namespace MyRESTServices.Models
{
    public class UserWithToken
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Token { get; set; }
    }
}