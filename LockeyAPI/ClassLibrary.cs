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
        public int ID { get; set; }

        public User(string username, string password, string deviceconnected)
        {
            Username = username;
            Password = password;
            DeviceConnected = deviceconnected;

        }
        public User(string username, string password, string deviceconnected, int id)
        {
            Username = username;
            Password = password;
            DeviceConnected=deviceconnected;    
            ID=id;
        }

        public User()
        {
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
        public Sensor(int id, bool islocked)
        {
            ID = id;
            islocked = IsLocked;
        }

        public int ID { get; set; }
        public bool IsLocked { get; set; }
        public DateTime Time { get; set; }
        public override string ToString()
        {
            return $"ID:{ID}, Is Locked: {IsLocked}";
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