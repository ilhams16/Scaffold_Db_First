using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyRESTServices.BLL.Interfaces;
using MyRESTServices.Data.Interfaces;
using MyRESTServices.Helpers;
using MyRESTServices.Models;

namespace MyRESTServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IUserBLL _userBLL;
        private readonly AppSettings _appSettings;
        public AccountsController(IUserBLL userBLL, IOptions<AppSettings> appSettings)
        {
            _userBLL = userBLL;
            _appSettings = appSettings.Value;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            var result = await _userBLL.Login(loginViewModel.Username, loginViewModel.Password);
            if (result != null)
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, result.Username));
                foreach (var role in result.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                
                var userWithToken = new UserWithToken
                {
                    Username = result.Username,
                    // Roles = result.Roles,
                    Token = tokenHandler.WriteToken(token)
                };
                return Ok(userWithToken);
            }
            else
            {
                return BadRequest("Invalid credentials");
            }
        }
        [HttpGet("claims")]
        public IActionResult GetClaims()
        {
            var claims = new List<string>();
            foreach (var claim in User.Claims)
            {
                claims.Add($"{claim.Type}: {claim.Value}");
            }
            return Ok(claims);
        }
    }
}