using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

namespace IVRMUX.Controllers.com.vapstech.HRMS
{
    [Produces("application/json")]
    [Route("api/HealthCardDetails")]
    public class HealthCardDetailsController : Controller
    {
        HealthCardDetailsDelegate _delg = new HealthCardDetailsDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("loaddata/{id:int}")]
        public HealthCardDetailsDTO loaddata(int id)
        {
            HealthCardDetailsDTO data = new HealthCardDetailsDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.loaddata(data);
        }
        [HttpPost]
        [Route("SaveDetails")]
        public HealthCardDetailsDTO SaveDetails([FromBody] HealthCardDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveDetails(data);
        }
        //HealthCardDetails
        [Route("OnChangeEmployee")]
        public HealthCardDetailsDTO OnChangeEmployee([FromBody] HealthCardDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeEmployee(data);
        }
        //Savemaster
        [Route("Savemaster")]
        public HealthCardMasterDTO Savemaster([FromBody] HealthCardMasterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.Savemaster(data);
        }
        //editmaster
        [Route("editmaster")]
        public HealthCardMasterDTO editmaster([FromBody] HealthCardMasterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.editmaster(data);
        }
        //deactiveM
        [Route("deactiveM")]
        public HealthCardMasterDTO deactiveM([FromBody] HealthCardMasterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.deactiveM(data);
        }
    }
}