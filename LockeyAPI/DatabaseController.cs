using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LockeyAPI
{
    public class DatabaseController
    {
        public const string connectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Lockey;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public ObservableCollection<Sensor> GetAllSensors()
        {
            string query = "select * from value";
            ObservableCollection<Sensor> mylist = new ObservableCollection<Sensor>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Sensor theSensor = new Sensor
                    {
                        ID = reader.GetInt32(0),
                        Value = reader.GetFloat(5),
                    };

                    mylist.Add(theSensor);
                }

                return mylist;
            }
        }

        public Sensor GetSensorByID(string id)
        {
            string query = "select * from Value where DeviceID=@id";
            Sensor returnSensor = new Sensor();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    returnSensor.ID = reader.GetInt32(0);
                    returnSensor.Value = reader.GetFloat(1);
                }

                return returnSensor;
            }
        }

        public void createReading(Sensor sensor)
        {
            string query = "insert into Value(DeviceID, Value) values(@id, @value)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@id", sensor.ID);
                command.Parameters.AddWithValue("@value", sensor.Value);
                int affectedRows = command.ExecuteNonQuery();
            }
        }

        public void deleteDevice(string id)
        {
            string query = "delete from Value where ID=@id"; //query here...
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@id", id);
                int affectedRows = command.ExecuteNonQuery();
            }
        }


        public ObservableCollection<User> GetAllUsers()
        {
            string query = "Select * From [User]";
            ObservableCollection<User> mylist = new ObservableCollection<User>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    User theUser = new User()
                    {
                        Username = reader.GetString(0),
                        Password = reader.GetString(1)
                    };
                    mylist.Add(theUser);
                }

                return mylist;
            }

        }

        public void createUser(User user)
        {
            string query = "insert into [User](username, password) values(@username, @password)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@username", user.Username);
                command.Parameters.AddWithValue("@password", user.Password);
                int affectedRows = command.ExecuteNonQuery();
            }
        }

        public void deleteUser(string username)
        {
            string query = "delete from [User] where username=@username";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@username", username);
                int affectedRows = command.ExecuteNonQuery();
            }
        }

        public User GetUser(string username)
        {
            string query = "select * from [User] where username=@username";
            User returnUser = new User();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@username", username);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    returnUser = new User()
                    {
                        Username = reader.GetString(0),
                        Password = reader.GetString(1),
                    };
                }

                return returnUser;
            }
        }
    }
}