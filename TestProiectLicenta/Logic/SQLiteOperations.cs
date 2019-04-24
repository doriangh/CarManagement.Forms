using System;
using System.Collections.ObjectModel;
using SQLite;
using TestProiectLicenta.Models;
using TestProiectLicenta.Persistence;
using Xamarin.Forms;

namespace TestProiectLicenta.Logic
{
    public class SQLiteOperations
    {
        readonly SQLiteConnection _connection = DependencyService.Get<ISQLiteDB>().GetConnection();

        public SQLiteOperations()
        {
            //_connection.CreateTable<User>();
            //_connection.CreateTable<Car>();
            //_connection.CreateTable<Session>();
            //_connection.CreateTable<CarDetails>();
        }

        public User GetUserByUsername(string username)
        {
            return _connection.Table<User>().FirstOrDefault(u => u.Username == username);
        }

        public ObservableCollection<Car> GetUserCars(int userId)
        {
            ObservableCollection<Car> cars = new ObservableCollection<Car>();

            foreach (var car in _connection.Table<Car>().Where(c => c.UserId == userId))
            {
                cars.Add(car);
            }
            return cars;
        }




    }
}
