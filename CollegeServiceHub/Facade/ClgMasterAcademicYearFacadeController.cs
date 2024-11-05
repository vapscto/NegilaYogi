using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeServiceHub.Interface;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class ClgMasterAcademicYearFacadeController : Controller
    {
        public ClgMasterAcademicYearInterface _infacc;
        public ClgMasterAcademicYearFacadeController(ClgMasterAcademicYearInterface infacc)
        {
            _infacc = infacc;
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
        [Route("getalldetails")]
        public ClgMasterAcademicYearDTO getalldetails([FromBody] ClgMasterAcademicYearDTO data)
        {
            return _infacc.getalldetails(data);
        }


        [Route("saveaccyear")]
        public ClgMasterAcademicYearDTO saveaccyear([FromBody] ClgMasterAcademicYearDTO data)
        {
            return _infacc.saveaccyear(data);
        }
        [Route("edit")]
        public ClgMasterAcademicYearDTO edit([FromBody] ClgMasterAcademicYearDTO data)
        {
            return _infacc.edit(data);
        }
        [Route("deactivate")]
        public ClgMasterAcademicYearDTO deactivate([FromBody] ClgMasterAcademicYearDTO data)
        {
            return _infacc.deactivate(data);
        }
        [Route("saveorder")]
        public ClgMasterAcademicYearDTO saveorder([FromBody] ClgMasterAcademicYearDTO data)
        {
            return _infacc.saveorder(data);
        }
        


       // POST api/values
       [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
