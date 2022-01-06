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
            //@"Data Source=(localdb)\ProjectsV13;Initial Catalog = Projects; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            @"Server=tcp:lockeysqlserver.database.windows.net,1433;Initial Catalog=lockeysql;Persist Security Info=False;User ID=Lockey;Password=Password1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        //User
        public List<User> GetAllUsers()
        {
            string query = "Select * From [User]";
            List<User> mylist = new List<User>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string x;
                    try
                    {
                        x=reader.GetString(3);
                    }
                    catch(System.Data.SqlTypes.SqlNullValueException)
                    {
                        x = "";
                    }
                    

                     User theUser = new User()
                    {
                        ID = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        Password = reader.GetString(2),
                        DeviceConnected = x
                    };
                    mylist.Add(theUser);
                }
            }
            return mylist;
        }

        public User GetUser(int userid)
        {
            string query = "select * from [User] where Id=@userid";
            User returnUser = new User();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@userid", userid);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string x;
                    try
                    {
                        x = reader.GetString(3);
                    }
                    catch (System.Data.SqlTypes.SqlNullValueException)
                    {
                        x = "";
                    }

                    returnUser = new User()
                    {

                        ID = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        Password = reader.GetString(2),
                        DeviceConnected = x
                    };
                }
            }
            return returnUser;
        }

        public User GetUserByUsername(string username)
        {
            string query = "select * from [User] where username=@userid";
            User returnUser = new User();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@userid", username);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string x;
                    try
                    {
                        x = reader.GetString(3);
                    }
                    catch (System.Data.SqlTypes.SqlNullValueException)
                    {
                        x = "";
                    }

                    returnUser = new User()
                    {

                        ID = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        Password = reader.GetString(2),
                        DeviceConnected = x
                    };
                }
            }
            return returnUser;
        }

        public void CreateUser(User user)
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

        public void DeleteUser(int userid)
        {
            string query = "delete from [User] where Id=@userid";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@userid", userid);
                int affectedRows = command.ExecuteNonQuery();
            }
        }

        public void SetDevicesToUser(string mac, int userid)
        {
            string query = "select deviceconnected from [User] where Id=@userid";
            string devicesreturn = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@userid", userid);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    
                    try
                    {
                       // reader.GetInt32(0);
                       // reader.GetString(1);
                       // reader.GetString(2);
                        devicesreturn = reader.GetString(0);
                    }
                    catch (System.Data.SqlTypes.SqlNullValueException)
                    {}
                    
                }
            }
            string query2 = "UPDATE[User] SET deviceconnected = @devices Where Id = @userid";
            string newdevicelist;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query2, conn);
                if (devicesreturn != "")
                    newdevicelist = devicesreturn + "&" + mac;
                else
                    newdevicelist = mac;
                command.Parameters.AddWithValue("@devices", newdevicelist);
                command.Parameters.AddWithValue("@userid", userid);
                int affectedRows = command.ExecuteNonQuery();
            }

        }
        public void DeleteDevicesToUser(string mac, int userid)
        {
            List<string> listOfDevices = GetDevice(userid);
            listOfDevices.Remove(mac);
            string devicesreturn = "";
            if (listOfDevices.Count != 0)
            {
                if (listOfDevices.Count == 1)
                { devicesreturn = listOfDevices[0]; }
                else
                {
                    foreach (string device in listOfDevices)
                    {
                        if(device!="")
                        devicesreturn = devicesreturn + device + '&';
                    }
                    devicesreturn = devicesreturn.Remove(devicesreturn.Length - 1, 1);
                }
                string query2 = "UPDATE[User] SET deviceconnected = @devices Where Id = @userid";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(query2, conn);
                    
                    command.Parameters.AddWithValue("@devices", devicesreturn);
                    command.Parameters.AddWithValue("@userid", userid);
                    int affectedRows = command.ExecuteNonQuery();
                }
            }
        }

        //Sensor
        public ObservableCollection<Sensor> GetAllSensors()
        {
            string query = "select * from [Values]";
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
                        ID = reader.GetString(1),
                        IsLocked = reader.GetBoolean(2),
                        Time = reader.GetDateTime(3)
                    };

                    mylist.Add(theSensor);
                }

                return mylist;
            }
        }

        public ObservableCollection<Sensor> GetSensorByID(string id)
        {
            string query = "select * from [Values] where DeviceID=@id";
            ObservableCollection<Sensor> returnSensor = new ObservableCollection<Sensor>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Sensor theSensor = new Sensor
                    {
                        ID = reader.GetString(1),
                        IsLocked = reader.GetBoolean(2),
                        Time = reader.GetDateTime(3)
                    };
                    returnSensor.Add(theSensor);
                }

                return returnSensor;
            }
        }

        public void createReading(Sensor sensor)
        {
            string query = "insert into [Values](DeviceID, IsLocked) values(@id, @value)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@id", sensor.ID);
                command.Parameters.AddWithValue("@value", sensor.IsLocked);
                int affectedRows = command.ExecuteNonQuery();
            }
        }

        public void deleteDevice(int id)
        {
            string query = "delete from [Values] where DeviceID=@id"; //query here...
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@id", id);
                int affectedRows = command.ExecuteNonQuery();
            }

        }

        public List<string> GetDevice(int userid)
        {
            string query = "select deviceconnected from [User] where Id=@userid";
            List<string> mylist = new List<string>();
            string devicesreturn;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@userid", userid);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    devicesreturn = reader.GetString(0);
                    string[] strings;
                    strings = devicesreturn.Split('&');
                    foreach (string s in strings)
                    {
                        mylist.Add(s);
                    }
                }

                return mylist;
            }
        }
    }
}