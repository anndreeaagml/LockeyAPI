using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LockeyAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        DatabaseController userAccess = new DatabaseController();
        // GET: api/User
        public IEnumerable<User> Get()
        {
            return userAccess.GetAllUsers();
        }

        [HttpGet]
        public User Get(int id)
        {
            return userAccess.GetUser(id);
        }

        [HttpGet]
        [Route("User/byUsername/{username}")]
        public User GetByUsername(string username)
        {
            return userAccess.GetUserByUsername(username);
        }

        [HttpGet]
        [Route("User/GetUserDevices/{userid}")]
        public IEnumerable<string> GetDevices(int userid)
        {
            return userAccess.GetDevice(userid);
        }

        // POST: api/User
        [HttpPost]
        public void Post([FromBody] User user)
        {
            userAccess.CreateUser(user);
        }

        [HttpDelete]
        public void Delete(int id)
        {
            userAccess.DeleteUser(id);
        }

        [HttpPut]
        [Route("User/AddDevice")]
        public void AddDevice([FromBody] User mac)
        {
            userAccess.SetDevicesToUser(mac.DeviceConnected, mac.ID);
        }

        [HttpPut]
        [Route("User/RemoveDevice")]
        public void RemoveDevice([FromBody] User mac)
        {
            userAccess.DeleteDevicesToUser(mac.DeviceConnected, mac.ID);
        }

        
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] User value)
        //{
        //    userAccess.SetDevicesToUser(id, value);
        //}
    }
}