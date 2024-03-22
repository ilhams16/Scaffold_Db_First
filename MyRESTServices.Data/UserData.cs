using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyRESTServices.Data.Interfaces;
using MyRESTServices.Domain.Models;

namespace MyRESTServices.Data
{
    public class UserData : IUserData
    {
        private readonly AppDbContext _context;
        public UserData(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Task> ChangePassword(string username, string newPassword)
        {
            try
            {
                // var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                // if (user == null)
                // {
                //     throw new ArgumentException("User not found");
                // }
                // user.Password = Helpers.Md5Hash.GetHash(newPassword);
                // await _context.SaveChangesAsync();
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex.Message}");
            }
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllWithRoles()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByUsername(string username)
        {
            var userLogin = await _context.Users.FindAsync(username);
            return userLogin;
        }

        public Task<User> GetUserWithRoles(string username)
        {
            throw new NotImplementedException();
        }

        public Task<User> Insert(User entity)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Login(string username, string password)
        {
            var userLogin = await _context.Users.Where(u => u.Username == username && u.Password == password).Include(u => u.Roles).FirstOrDefaultAsync();
            return userLogin;
        }

        public Task<User> Update(int id, User entity)
        {
            throw new NotImplementedException();
        }
    }
}