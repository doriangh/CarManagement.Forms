using System.Collections.Generic;
using System.Threading.Tasks;
using TestProiectLicenta.Interfaces.Services;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Services
{
    public class UserManager
    {
        readonly IUserService _service;

        public UserManager(IUserService service)
        {
            _service = service;
        }

        public Task LogIn(string username, string password)
        {
            return _service.LogIn(username, password);
        }

        public Task<List<User>> GetUsersAsync()
        {
            return _service.GetUsersAsync();
        }

        public Task<User> GetUserById(int id)
        {
            return _service.GetUserById(id);
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

        public Task<User> GetUserByUsername(string username)
        {
            return _service.GetUserByUsername(username);
        }

        public Task<bool> CheckLogIn()
        {
            return _service.CheckLogIn();
        }
    }

}
