using System.Collections.Generic;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetUsers();
        User GetUserById(int id);
        void AddUser(User user);
        void DeleteUser(int id);
        void UpdateUser(User user);
        User GetUserByUsername(string username);
    }
}
