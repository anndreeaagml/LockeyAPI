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
        [Route("User/AddDevice/{userid}")]
        public void AddDevice([FromBody] JToken mac,int userid)
        {
            //string mac2 = JsonConvert.DeserializeObject();
            string mac2 = mac.First.Last().ToString();
            userAccess.SetDevicesToUser(mac2, userid);
        }

        [HttpPut]
        [Route("User/RemoveDevice/{userid}")]
        public void RemoveDevice(int userid, [FromBody] JToken mac)
        {
            string mac2 = mac.First.Last().ToString();
            userAccess.DeleteDevicesToUser(mac2, userid);
        }
        [HttpPut]
        [Route("User/GetUserDevices/{userid}")]
        public IEnumerable<string> GetDevices( int id)
        {
            return userAccess.GetDevice(id);
        }
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] User value)
        //{
        //    userAccess.SetDevicesToUser(id, value);
        //}
    }
}