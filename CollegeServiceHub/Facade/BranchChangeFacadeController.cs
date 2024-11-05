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
    public class BranchChangeFacadeController : Controller
    {
        private BranchChangeInterface _inter;

        public BranchChangeFacadeController(BranchChangeInterface obj)
        {
            _inter = obj;
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
        public void Post([FromBody]string value)
        {
        }
        [Route("getdetails")]
        public BranchChangeDTO getdetails([FromBody] BranchChangeDTO data)
        {
            return _inter.getdetails(data);
        }
        [HttpPost]
        [Route("Studentdetails")]
        public BranchChangeDTO Studentdetails([FromBody] BranchChangeDTO data)
        {
            return _inter.Studentdetails(data);
        }
        [Route("Savedetails")]
        public BranchChangeDTO Savedetails([FromBody] BranchChangeDTO data)
        {
            return _inter.Savedetails(data);
        }
        [Route("deactive")]
        public BranchChangeDTO deactive([FromBody] BranchChangeDTO data)
        {
            return _inter.deactive(data);
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
