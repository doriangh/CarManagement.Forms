using System;
using System.IO;
using SQLite;
using TestProiectLicenta.iOS.Persistence;
using TestProiectLicenta.Persistence;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDB))]
namespace TestProiectLicenta.iOS.Persistence
{
    public class SQLiteDB : ISQLiteDB
    {
        public SQLiteConnection GetConnection()
        {
            string fileName = "MySQLite.db3";
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libraryFolder = Path.Combine(folder, "..", "Library");
            var path = Path.Combine(libraryFolder, fileName);

            if (!File.Exists(path))
            {
                File.Create(path);
            }
            return new SQLiteConnection(path);
        }
    }
}
