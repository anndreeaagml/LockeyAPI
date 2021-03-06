using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LockeyAPI.Controllers
{
    [EnableCors(origins: "https://lockeyapi.azurewebsites.net/", headers: "*", methods: "*")]
    public class SensorController : ApiController
    {

        DatabaseController userAccess = new DatabaseController();
        // GET: api/<DeviceController>
        [HttpGet]
        public IEnumerable<Sensor> Get()
        {
            return userAccess.GetAllSensors();
        }

        // GET api/<DeviceController>/5
        [HttpGet]
        public ObservableCollection<Sensor> Get(string deviceid)
        {
            return userAccess.GetSensorByID(deviceid);
        }

        // POST api/<DeviceController>
        [HttpPost]
        public void Post([FromBody] Sensor value)
        {
            userAccess.createReading(value);
        }

        // PUT api/<DeviceController>/5
        [HttpPut]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DeviceController>/5
        [HttpDelete]
        public void Delete(int id)
        {
            userAccess.deleteDevice(id);
        }
    }
}
