using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using TestProiectLicenta.Models;
using Xamarin.Forms;

namespace TestProiectLicenta.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<CarDetail> CarDetails { get; set; }

        private const string DBName = "CarManagement.db";

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            String DBPath = "";
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    SQLitePCL.Batteries_V2.Init();
                    DBPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", DBName);
                    break;
                case Device.Android:
                    DBPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DBName);
                    break;
                default:
                    throw new NotImplementedException("Platform not supported");
            }

            builder.UseSqlite($"Filename={DBPath}");
        }
    }
}
