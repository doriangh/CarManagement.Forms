using System;
using SQLite;

namespace TestProiectLicenta.Persistence
{
    public interface ISQLiteDB
    {
       SQLiteConnection GetConnection();
    }
}
