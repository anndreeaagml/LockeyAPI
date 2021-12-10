using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LockeyAPI.Controllers
{
    [EnableCors(origins: "https://lockeyapi.azurewebsites.net/", headers: "*", methods: "*")]
    [Route("api/[controller]")]
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
        [Route("AddDevice/{userid}")]
        public void AddDevice(int deciveId, int userid)
        {
            userAccess.SetDevicesToUser(deciveId, userid);
        }

        [HttpPut]
        [Route("RemoveDevice/{userid}")]
        public void RemoveDevice(int deciveId, int userid)
        {
            userAccess.DeleteDevicesToUser(deciveId, userid);
        }

        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] User value)
        //{
        //    userAccess.SetDevicesToUser(id, value);
        //}
    }
}