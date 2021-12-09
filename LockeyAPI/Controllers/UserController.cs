using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace LockeyAPI.Controllers
{
    public class UserController : ApiController
    {
        DatabaseController userAccess = new DatabaseController();
        // GET: api/User
        public IEnumerable<User> Get()
        {
            return userAccess.GetAllUsers();
        }

        [HttpGet]
        [Route("api/User/{username}")]
        public User Get(string username)
        {
            return userAccess.GetUser(username);
        }


        // POST: api/User
        public void Post([FromBody] User user)
        {
            userAccess.createUser(user);
        }


        [HttpDelete]
        [Route("api/User/{username}")]
        public void Delete(string username)
        {
            userAccess.deleteUser(username);
        }
    }
}