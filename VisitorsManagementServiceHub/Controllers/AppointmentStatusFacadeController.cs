using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using VisitorsManagementServiceHub.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VisitorsManagementServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class AppointmentStatusFacadeController : Controller
    {
        AppointmentStatusInterface interobj;
        public AppointmentStatusFacadeController(AppointmentStatusInterface obj)
        {
            interobj = obj;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        [Route("getDetails")]
        public AppointmentStatusDTO getDetails([FromBody]AppointmentStatusDTO data)
        {
            return interobj.getDetails(data);
        }
        [Route("EditDetails")]
        public AppointmentStatusDTO EditDetails([FromBody] AppointmentStatusDTO id)
        {
            return interobj.EditDetails(id);
        }
        [Route("saveData")]
        public Task<AppointmentStatusDTO> saveData([FromBody]AppointmentStatusDTO data)
        {
            return interobj.saveDataAsync(data);
        }
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
