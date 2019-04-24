using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using TestProiectLicenta.Data.Interfaces;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        readonly AppDbContext _context;

        readonly HttpClient _client;

        public UserRepository()
        {
            _client = new HttpClient();
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            _context.Users.Remove(GetUserById(id));
        }

        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
