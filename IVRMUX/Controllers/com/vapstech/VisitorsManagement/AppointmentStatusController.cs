using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VisitorsManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VisitorsManagement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VisitorsManagement
{
    [Route("api/[controller]")]
    public class AppointmentStatusController : Controller
    {
        AppointmentStatusDelegate delobj = new AppointmentStatusDelegate();
        // GET: api/<controller>
        [HttpGet]

        [Route("getDetails/{id:int}")]
        public AppointmentStatusDTO getDetails(int id)
        {
            AppointmentStatusDTO dto = new AppointmentStatusDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.getDetails(dto);
        }
        [Route("Edit")]
        public AppointmentStatusDTO Edit([FromBody] AppointmentStatusDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.EditDetails(dto);
        }
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
        [Route("saveData")]
        public AppointmentStatusDTO saveData([FromBody]AppointmentStatusDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.saveData(data);
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
