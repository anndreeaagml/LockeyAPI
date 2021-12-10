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
            @"Server=tcp:lockeyserver.database.windows.net,1433;Initial Catalog=lockeydata;Persist Security Info=False;User ID=lockey;Password=Password1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

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
                    User theUser = new User()
                    {
                        Username = reader.GetString(1),
                        Password = reader.GetString(2)
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
                    returnUser = new User()
                    {
                        Username = reader.GetString(1),
                        Password = reader.GetString(2)
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
            string query = "delete from [User] where id=@userid";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@userid", userid);
                int affectedRows = command.ExecuteNonQuery();
            }
        }

        public void SetDevicesToUser(int deciveId, int userid)
        {
            string query = "select deviceconnection from [User] where id=@userid";
            string devicesreturn = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@userid", userid);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    devicesreturn = reader.GetString(3);

                }
            }
            string query2 = "insert into [User](deviceconnected) values(@devices) where id=@userid";
            string newdevicelist;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query2, conn);
                if (devicesreturn != "")
                    newdevicelist = devicesreturn + "&" + deciveId;
                else
                    newdevicelist = deciveId.ToString();
                command.Parameters.AddWithValue("@devices", newdevicelist);
                command.Parameters.AddWithValue("@userid", userid);
                int affectedRows = command.ExecuteNonQuery();
            }

        }
        public void DeleteDevicesToUser(int deciveId, int userid)
        {
            ObservableCollection<int> listOfDevices = GetDevice(userid);
            listOfDevices.Remove(deciveId);
            string newdevicelist;
            string devicesreturn = "";
            if (listOfDevices.Count != 0)
            {
                foreach (int device in listOfDevices)
                {
                    devicesreturn = devicesreturn + device + '&';
                }
                devicesreturn.Remove(devicesreturn.Length - 1, 1);
                string query2 = "insert into [User](deviceconnected) values(@devices) where id=@userid";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(query2, conn);
                    if (devicesreturn != "")
                        newdevicelist = devicesreturn + "&" + deciveId;
                    else
                        newdevicelist = deciveId.ToString();
                    command.Parameters.AddWithValue("@devices", newdevicelist);
                    command.Parameters.AddWithValue("@id", userid);
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
                        ID = reader.GetInt32(0),
                        IsLocked = reader.GetBoolean(1),
                    };

                    mylist.Add(theSensor);
                }

                return mylist;
            }
        }

        public Sensor GetSensorByID(int id)
        {
            string query = "select * from [Values] where DeviceID=@id";
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
                    returnSensor.IsLocked = reader.GetBoolean(1);
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

        public ObservableCollection<int> GetDevice(int userid)
        {
            string query = "select deviceconnection from [User] where id=@userid";
            ObservableCollection<int> mylist = new ObservableCollection<int>();
            string devicesreturn;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@userid", userid);
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
    }
}