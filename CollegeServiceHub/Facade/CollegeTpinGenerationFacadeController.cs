using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class CollegeTpinGenerationFacadeController : Controller
    {
        public CollegeTpinGenerationInterface _interface;
        public CollegeTpinGenerationFacadeController(CollegeTpinGenerationInterface _inter)
        {
            _interface = _inter;
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

        [Route("loaddata")]
        public CollegeTpinGenerationDTO loaddata([FromBody] CollegeTpinGenerationDTO data)
        {
            return _interface.loaddata(data);
        }
        [Route("search")]
        public CollegeTpinGenerationDTO search([FromBody] CollegeTpinGenerationDTO data)
        {
            return _interface.search(data);
        }
        [Route("generatetpin")]
        public CollegeTpinGenerationDTO generatetpin([FromBody] CollegeTpinGenerationDTO data)
        {
            return _interface.generatetpin(data);
        }
    }
}
