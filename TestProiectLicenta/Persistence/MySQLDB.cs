using System;
using MySql.Data.MySqlClient;

namespace TestProiectLicenta.Persistence
{
    public class MySQLDB
    {
        MySqlConnection conn;

        public MySqlConnection GetConnection()
        {
            conn = new MySqlConnection("server=golar3.go.ro;uid=carManagement;pwd=59885236;database=CarManagement");
            conn.Open();
            return conn;   
        }

        public void CloseConnection()
        {
            conn.Close();
        }


    }
}
