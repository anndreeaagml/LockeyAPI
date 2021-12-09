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
            @"Server=tcp:lockeyserver.database.windows.net,1433;Initial Catalog=lockeydata;Persist Security Info=False;User ID=lockey;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

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
                        Value = reader.GetFloat(2),
                    };

                    mylist.Add(theSensor);
                }

                return mylist;
            }
        }

        public Sensor GetSensorByID(int id)
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

        public void deleteDevice(int id)
        {
            string query = "delete from Value where DeviceID=@id"; //query here...
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
                        Username = reader.GetString(1),
                        Password = reader.GetString(2)
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
                        Username = reader.GetString(1),
                        Password = reader.GetString(2),
                    };
                }

                return returnUser;
            }
        }
        public ObservableCollection<int> GetDevice(string user)
        {
            string query = "select deviceconnection from [User] where username=@username";
            ObservableCollection<int> mylist = new ObservableCollection<int>();
            string devicesreturn;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@username", user);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    devicesreturn = reader.GetString(3);
                    string[] strings;
                    strings = devicesreturn.Split('&');
                    foreach (string s in strings)
                    {
                        mylist.Add(int.Parse(s));
                    }
                }

                return mylist;
            }
        }

        public void SetDevicesToUser(int id, string user)
        {
            string query = "select deviceconnection from [User] where username=@username";
            string devicesreturn = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@username", user);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    devicesreturn = reader.GetString(3);

                }
            }
            string query2 = "insert into [User](deviceconnected) values(@devices) where username=@username";
            string newdevicelist;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query2, conn);
                if (devicesreturn != "")
                    newdevicelist = devicesreturn + "&" + id;
                else
                    newdevicelist = id.ToString();
                command.Parameters.AddWithValue("@devices", newdevicelist);
                command.Parameters.AddWithValue("@username", user);
                int affectedRows = command.ExecuteNonQuery();
            }

        }
        public void DeleteDevicesToUser(int id, string user)
        {
            ObservableCollection<int> listOfDevices = GetDevice(user);
            listOfDevices.Remove(id);
            string newdevicelist;
            string devicesreturn = "";
            if (listOfDevices.Count != 0)
            {
                foreach (int device in listOfDevices)
                {
                    devicesreturn = devicesreturn + device + '&';
                }
                devicesreturn.Remove(devicesreturn.Length - 1, 1);
                string query2 = "insert into [User](deviceconnected) values(@devices) where username=@username";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(query2, conn);
                    if (devicesreturn != "")
                        newdevicelist = devicesreturn + "&" + id;
                    else
                        newdevicelist = id.ToString();
                    command.Parameters.AddWithValue("@devices", newdevicelist);
                    command.Parameters.AddWithValue("@username", user);
                    int affectedRows = command.ExecuteNonQuery();
                }
            }
        }

    }
}