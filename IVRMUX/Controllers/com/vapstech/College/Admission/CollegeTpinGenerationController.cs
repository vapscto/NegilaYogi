using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class CollegeTpinGenerationController : Controller
    {
        CollegeTpinGenerationDelegate _delg = new CollegeTpinGenerationDelegate();

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

        [Route("loaddata/{id:int}")]
        public CollegeTpinGenerationDTO loaddata(int id)
        {
            CollegeTpinGenerationDTO data = new CollegeTpinGenerationDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.loaddata(data);
        }
        [Route("search")]
        public CollegeTpinGenerationDTO search([FromBody] CollegeTpinGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.search(data);
        }
        [Route("generatetpin")]
        public CollegeTpinGenerationDTO generatetpin([FromBody] CollegeTpinGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.generatetpin(data);
        }
    }
}
