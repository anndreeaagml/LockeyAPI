using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace LockeyAPI
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string DeviceConnected { get; set; }

        public User(string username, string password, int deviceconnected)
        {
            Username = username;
            Password = password;

        }

        public User()
        {
        }

        public void SetUser(string user, string pass, string deviceconnected)
        {
            Username = user;
            Password = pass;
            DeviceConnected = deviceconnected;
        }

        public override string ToString()
        {
            return $"Username: {Username}, Device Connected: {DeviceConnected}";
        }

    }

    public class UserCatalog
    {
        private static ObservableCollection<User> List = new ObservableCollection<User>() { };
        public UserCatalog() { }
        public ObservableCollection<User> GetAll()
        {
            return List;
        }

        public void Update(ObservableCollection<User> NewL)
        {
            List = NewL;
        }
    }

    public class Sensor
    {
        public Sensor()
        {

        }
        public Sensor(int id, float value)
        {
            ID = id;
            value = Value;
        }

        public int ID { get; set; }
        public float Value { get; set; }

        public override string ToString()
        {
            return $"ID:{ID}, Value: {Value}";
        }

    }

    public class SensorCatalog
    {
        private static ObservableCollection<Sensor> List = new ObservableCollection<Sensor>()
        { };

        public SensorCatalog()
        {
        }


        public ObservableCollection<Sensor> GetAll()
        {
            return List;
        }

        public void Update(ObservableCollection<Sensor> NewL)
        {
            List = NewL;
        }

    }
}