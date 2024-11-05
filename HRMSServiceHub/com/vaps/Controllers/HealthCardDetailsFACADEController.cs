using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMSServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Produces("application/json")]
    [Route("api/HealthCardDetailsFACADE")]
    public class HealthCardDetailsFACADEController : Controller
    {
        public HealthCardDetailsInterface _interface;

        public HealthCardDetailsFACADEController(HealthCardDetailsInterface _inter)
        {
            _interface = _inter;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("loaddata")]
        public HealthCardDetailsDTO loaddata([FromBody] HealthCardDetailsDTO data)
        {
            return _interface.loaddata(data);
        }
        [Route("SaveDetails")]
        public HealthCardDetailsDTO SaveDetails([FromBody] HealthCardDetailsDTO data)
        {
            return _interface.SaveDetails(data);
        }
        //OnChangeEmployee
        [Route("OnChangeEmployee")]
        public HealthCardDetailsDTO OnChangeEmployee([FromBody] HealthCardDetailsDTO data)
        {
            return _interface.OnChangeEmployee(data);
        }
        [Route("Savemaster")]
        public HealthCardMasterDTO Savemaster([FromBody]HealthCardMasterDTO data)
        {
            return _interface.Savemaster(data);
            
        }
        [Route("editmaster")]
        public HealthCardMasterDTO editmaster([FromBody]HealthCardMasterDTO data)
        {
            return _interface.editmaster(data);

        }
        [Route("deactiveM")]
        public HealthCardMasterDTO deactiveM([FromBody]HealthCardMasterDTO data)
        {
            return _interface.deactiveM(data);

        }
        //editmaster
    }
}