using System;
using MySql.Data.MySqlClient;
using TestProiectLicenta.Models;
using TestProiectLicenta.Persistence;

namespace TestProiectLicenta.Logic
{
    public class MySQLOperations
    {
        string imgurId = "3998115b75eb6f3";
        string imgurSecret = "17246fb9c2e052d96773af41fdf5091b7ba71603";

        MySQLDB _conn = new MySQLDB();

        public User GetByUsername(string username)
        {
            MySqlCommand cmd = _conn.GetConnection().CreateCommand();
            cmd.CommandText = "SELECT * FROM Users WHERE Username = ?Username";
            cmd.Parameters.AddWithValue("?Username", username);
            var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                var user = new User
                {
                    Id = Convert.ToInt16(reader["Id"].ToString()),
                    Name = reader["Name"].ToString(),
                    Age = Convert.ToInt16(reader["Age"].ToString()),
                    Username = reader["Username"].ToString(),
                    Password = reader["Password"].ToString(),
                };

                if (reader.IsDBNull(reader.GetOrdinal("UserImage")))
                {
                    Console.Write("No image");
                }
                else
                {
                    user.UserImage = reader["UserImage"].ToString();
                }

                reader.Close();
                _conn.CloseConnection();
                return user;
            }

            reader.Close();
            _conn.CloseConnection();
            return null;
        }

        public bool CheckIfUsernameExists(String username)
        {
            MySqlCommand cmd = _conn.GetConnection().CreateCommand();
            cmd.CommandText = "SELECT Username FROM Users";
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader["Username"].ToString() == username)
                {
                    reader.Close();
                    _conn.CloseConnection();
                    return true;
                }
            }
            reader.Close();
            _conn.CloseConnection();
            return false;
        }

        public void AddUser(User user)
        {
            MySqlCommand cmd = _conn.GetConnection().CreateCommand();
            cmd.CommandText = "INSERT INTO Users(Name, Age, Username, Password, UserImage) VALUES (?Name, ?Age, ?Username, ?Password, ?UserImage)";
            cmd.Parameters.AddWithValue("?Name", user.Name);
            cmd.Parameters.AddWithValue("?Age", user.Age);
            cmd.Parameters.AddWithValue("?Username", user.Username);
            cmd.Parameters.AddWithValue("?Password", user.Password);
            cmd.Parameters.AddWithValue("?UserImage", user.UserImage);
            cmd.ExecuteNonQuery();
            _conn.CloseConnection();
        }

        public void AddCar(Car car)
        {
            MySqlCommand cmd = _conn.GetConnection().CreateCommand();
            cmd.CommandText = "INSERT INTO Cars(UserId, Make, Manufacturer, Plant, ModelYear, Model, Body, Drive, NumberofSeats, NumberofDoors, Steering, EngineDisplacement, EngineCylinders, " +
            	"NumberofGears, Engine, Made, Color, Fuel, CC, Power, Emissions, Odometer, VIN, License) VALUES (?UserId, ?Make, ?Manufacturer, ?Plant, ?ModelYear, ?Model, ?Body, ?Drive, ?NumberofSeats, " +
            	"?NumberofDoors, ?Steering, ?EngineDisplacement, ?EngineCylinders, ?NumberofGears, ?Engine, ?Made, ?Color, ?Fuel, ?CC, ?Power, ?Emissions, ?Odometer, ?VIN, ?License)";
            cmd.Parameters.AddWithValue("?UserId", car.UserId);
            cmd.Parameters.AddWithValue("?Make", car.Make);
            cmd.Parameters.AddWithValue("?Manufacturer", car.Manufacturer);
            cmd.Parameters.AddWithValue("?Plant", car.Plant);
            cmd.Parameters.AddWithValue("?ModelYear", car.ModelYear);
            cmd.Parameters.AddWithValue("?Model", car.Model);
            cmd.Parameters.AddWithValue("?Body", car.Body);
            cmd.Parameters.AddWithValue("?Drive", car.Drive);
            cmd.Parameters.AddWithValue("?NumberofSeats", car.NumberofSeats);
            cmd.Parameters.AddWithValue("?NumberofDoors", car.NumberofDoors);
            cmd.Parameters.AddWithValue("?Steering", car.Steering);
            cmd.Parameters.AddWithValue("?EngineDisplacement", car.EngineDisplacement);
            cmd.Parameters.AddWithValue("?EngineCylinders", car.EngineCylinders);
            cmd.Parameters.AddWithValue("?NumberofGears", car.NumberofGears);
            cmd.Parameters.AddWithValue("?Engine", car.Engine);
            cmd.Parameters.AddWithValue("?Made", car.Made);
            cmd.Parameters.AddWithValue("?Color", car.Color);
            cmd.Parameters.AddWithValue("?Fuel", car.Fuel);
            cmd.Parameters.AddWithValue("?CC", car.CC);
            cmd.Parameters.AddWithValue("?Power", car.Power);
            cmd.Parameters.AddWithValue("?Emissions", car.Emissions);
            cmd.Parameters.AddWithValue("?Odometer", car.Odometer);
            cmd.Parameters.AddWithValue("?VIN", car.VIN);
            cmd.Parameters.AddWithValue("?License", car.License);
            cmd.ExecuteNonQuery();
            _conn.CloseConnection();
        }

        public void DeleteCar(Car car)
        {
            MySqlCommand cmd = _conn.GetConnection().CreateCommand();
            cmd.CommandText = "DELETE FROM Cars WHERE Id = ?Id";
            cmd.Parameters.AddWithValue("?Id", car.Id);
            cmd.ExecuteNonQuery();
            _conn.CloseConnection();
        }

        public void UpdateCar(Car car)
        {
            MySqlCommand cmd = _conn.GetConnection().CreateCommand();
            cmd.CommandText = "UPDATE Cars SET VIN = ?VIN WHERE Id = ?Id";
            cmd.Parameters.AddWithValue("?VIN", car.VIN);
            cmd.Parameters.AddWithValue("?Id", car.Id);
            cmd.ExecuteNonQuery();
            _conn.CloseConnection();
        }
    }
}
