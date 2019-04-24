using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Interfaces.Services
{
    public interface IUserService
    {

        Task LogIn(string username, string password);
        Task<List<User>> GetUsersAsync();
        Task<User> GetUserById(int id);
        Task AddUser(User user);
        Task DeleteUser(int id);
        Task UpdateUser(User user);
        Task<User> GetUserByUsername(string username);
        Task<bool> CheckLogIn();

    }
}
