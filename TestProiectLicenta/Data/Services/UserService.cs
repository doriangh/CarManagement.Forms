using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TestProiectLicenta.Data.Interfaces;
using TestProiectLicenta.Models;
using Xamarin.Essentials;

namespace TestProiectLicenta.Data.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;

        public UserService()
        {
            _client = new HttpClient();
        }

        public async Task LogIn(string username, string password)
        {
            var session = new UserSession
            {
                Username = username,
                Password = password
            };

            var json = JsonConvert.SerializeObject(session);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(Constants.webAPI + "Session", content);

            if (response.IsSuccessStatusCode)
            {
                var newContent = await response.Content.ReadAsStringAsync();
                var responseJson = JsonConvert.DeserializeObject<Session>(newContent);

                if (responseJson.Success)
                {
                    await SecureStorage.SetAsync("UserId", responseJson.UserId.ToString());
                    await SecureStorage.SetAsync("session_key", responseJson.Key);
                }
            }
        }

        public async Task AddUser(User user)
        {
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(Constants.webAPI + "Users", content);

            if (response.IsSuccessStatusCode) Debug.WriteLine("User successfully saved");
        }

        public async Task DeleteUser(int id)
        {
            var response = await _client.DeleteAsync(string.Format(Constants.webAPI + "Users/{0}", id));

            if (response.IsSuccessStatusCode) Debug.WriteLine("User deleted");
        }

        public async Task<User> GetUserById(int id)
        {
            var response = await _client.GetAsync(string.Format(Constants.webAPI + "Users/{0}", id));

            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<User>(content);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var users = await GetUsersAsync();
            foreach (var user in users)
                if (user.Username == username)
                    return user;
            return null;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var response = await _client.GetAsync(Constants.webAPI + "Users");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<User>>(content);
                foreach (var user in users) Console.WriteLine(user.Name);
                return users;
            }

            Console.WriteLine("Error");
            return null;
        }

        public async Task UpdateUser(User user)
        {
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync(Constants.webAPI + "Users", content);

            if (response.IsSuccessStatusCode) Debug.WriteLine("User successfully saved");
        }

        public async Task<bool> CheckLogIn()
        {
            var sessionKey = await SecureStorage.GetAsync("session_key");
            var userId = await SecureStorage.GetAsync("UserId");

            if (sessionKey == null || userId == null) return false;

            var response =
                await _client.GetAsync(string.Format(Constants.webAPI + "Session?UserId={0}&Key={1}", userId,
                    sessionKey));

            return response.IsSuccessStatusCode;
        }
    }
}