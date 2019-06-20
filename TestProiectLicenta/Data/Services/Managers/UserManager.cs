using System.Collections.Generic;
using System.Threading.Tasks;
using TestProiectLicenta.Data.Interfaces;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Services
{
    public class UserManager
    {
        private readonly IUserService _service;

        public UserManager(IUserService service)
        {
            _service = service;
        }

        public Task LogIn(string username, string password)
        {
            return _service.LogIn(username, password);
        }

        public Task<List<User>> GetUsersAsync(bool force = false)
        {
            return _service.GetUsersAsync(force);
        }

        public Task<User> GetUserById(int id, bool force = false)
        {
            return _service.GetUserById(id, force);
        }

        public Task AddUser(User user)
        {
            return _service.AddUser(user);
        }

        public Task DeleteUser(int id)
        {
            return _service.DeleteUser(id);
        }

        public Task UpdateUser(User user)
        {
            return _service.UpdateUser(user);
        }

        public Task<User> GetUserByUsername(string username, bool force = false)
        {
            return _service.GetUserByUsername(username, force);
        }

        public Task<bool> CheckLogIn()
        {
            return _service.CheckLogIn();
        }
    }
}