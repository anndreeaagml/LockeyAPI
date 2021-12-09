using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LockeyAPI.Controllers
{
    public class ValuesController : ApiController
    {
        DatabaseController userAccess = new DatabaseController();
        // GET: api/User
        public Sensor Get(int deviceid)
        {
            return userAccess.GetSensorByID(deviceid);
        }

        [HttpGet]
        [Route("api/Values/{username}")]
        public IEnumerable<int> GetDevicesConnectedToUser(string username)
        {
            return userAccess.GetDevice(username);
        }


        // POST: api/User
        public void Post([FromBody] Sensor sensor)
        {
            userAccess.createReading(sensor);
        }


        [HttpDelete]
        [Route("api/Values/{id}")]
        public void Delete(int id)
        {
            userAccess.deleteDevice(id);
        }
        

    }
}
