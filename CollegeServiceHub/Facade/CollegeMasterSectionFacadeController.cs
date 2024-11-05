using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using CollegeServiceHub.Interface;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class CollegeMasterSectionFacadeController : Controller
    {

        public CollegeMasterSectionInterface _inter;
        public CollegeMasterSectionFacadeController(CollegeMasterSectionInterface inter)
        {
            _inter = inter;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("getalldetails/{id:int}")]
        public CollegeMasterSectionDTO getalldetails(int id)
        {           
            return _inter.getalldetails(id);
        }
        [Route("Editdetails")]
        public CollegeMasterSectionDTO Editdetails([FromBody] CollegeMasterSectionDTO id)
        {           
            return _inter.Editdetails(id);
        }

        [Route("saveMasterdata")]
        public CollegeMasterSectionDTO saveMasterdata([FromBody] CollegeMasterSectionDTO id)
        {           
            return _inter.saveMasterdata(id);
        }
        [Route("saveorder")]
        public CollegeMasterSectionDTO saveorder([FromBody] CollegeMasterSectionDTO id)
        {            
            return _inter.saveorder(id);
        }
        [Route("Deletedetails")]
        public CollegeMasterSectionDTO Deletedetails([FromBody] CollegeMasterSectionDTO id)
        {            
            return _inter.Deletedetails(id);
        }
    }
}
