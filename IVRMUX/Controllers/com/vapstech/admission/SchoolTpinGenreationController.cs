using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.admission
{
    [Route("api/[controller]")]
    public class SchoolTpinGenreationController : Controller
    {
        SchoolTpinGenreationDelegate _delg = new SchoolTpinGenreationDelegate();
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
        public SchoolTpinGenreationDTO loaddata (int id)
        {
            SchoolTpinGenreationDTO data = new SchoolTpinGenreationDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.loaddata(data);
        }
        [Route("search")]
        public SchoolTpinGenreationDTO search([FromBody] SchoolTpinGenreationDTO data )
        {            
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.search(data);
        }
        [Route("generatetpin")]
        public SchoolTpinGenreationDTO generatetpin([FromBody] SchoolTpinGenreationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.generatetpin(data);
        }
        
    }
}
