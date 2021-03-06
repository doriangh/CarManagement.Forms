﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Interfaces
{
    public interface IUserService
    {
        Task LogIn(string username, string password);
        Task<List<User>> GetUsersAsync(bool force = false);
        Task<User> GetUserById(int id, bool force = false);
        Task AddUser(User user);
        Task DeleteUser(int id);
        Task UpdateUser(User user);
        Task<User> GetUserByUsername(string username, bool force = false);
        Task<bool> CheckLogIn();
    }
}