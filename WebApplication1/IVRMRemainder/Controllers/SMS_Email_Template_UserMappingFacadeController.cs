using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using WebApplication1.IVRMRemainder.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.IVRMRemainder.Controllers
{
    [Route("api/[controller]")]
    public class SMS_Email_Template_UserMappingFacadeController : Controller
    {
        public SMS_Email_Template_UserMappingInterface _interface; 

        public SMS_Email_Template_UserMappingFacadeController(SMS_Email_Template_UserMappingInterface _inter)
        {
            _interface = _inter;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("OnChangeOfInstitution")]
        public IVRM_RemaindersDTO OnChangeOfInstitution([FromBody] IVRM_RemaindersDTO data)
        {
            return _interface.OnChangeOfInstitution(data);
        }

        [Route("OnSaveUserMapping")]
        public IVRM_RemaindersDTO OnSaveUserMapping([FromBody] IVRM_RemaindersDTO data)
        {
            return _interface.OnSaveUserMapping(data);
        }
    }
}
