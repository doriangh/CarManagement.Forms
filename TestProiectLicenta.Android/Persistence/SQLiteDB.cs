using System;
using System.IO;
using SQLite;
using TestProiectLicenta.Droid.Persistence;
using TestProiectLicenta.Persistence;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDB))]
namespace TestProiectLicenta.Droid.Persistence
{
    public class SQLiteDB : ISQLiteDB
    {
        public SQLiteConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentsPath, "MySQLite.db3");

            return new SQLiteConnection(path);
        }
    }
}
