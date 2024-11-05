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
    public class ClgyearlycoursemappingFacadeController : Controller
    {
        public ClgyearlycoursemappingInterface _intgf;
        public ClgyearlycoursemappingFacadeController(ClgyearlycoursemappingInterface intgf)
        {
            _intgf = intgf;
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
        [HttpPost]
        [Route("getalldetails")]
        public ClgyearlycoursemappingDTO getalldetails([FromBody]ClgyearlycoursemappingDTO data)
        {
            return _intgf.getalldetails(data);
        }
        [Route("getbranches")]
        public ClgyearlycoursemappingDTO getbranches([FromBody]ClgyearlycoursemappingDTO data)
        {
            return _intgf.getbranches(data);
        }
        [Route("getsemisters")]
        public ClgyearlycoursemappingDTO getsemisters([FromBody]ClgyearlycoursemappingDTO data)
        {
            return _intgf.getsemisters(data);
        }
        [Route("savedata")]
        public ClgyearlycoursemappingDTO savedata([FromBody]ClgyearlycoursemappingDTO data)
        {
            return _intgf.savedata(data);
        }
        [Route("searchdata")]
        public ClgyearlycoursemappingDTO searchdata([FromBody]ClgyearlycoursemappingDTO data)
        {
            return _intgf.searchdata(data);
        }

        [Route("viewrecordspopup")]
        public ClgyearlycoursemappingDTO viewrecordspopup([FromBody]ClgyearlycoursemappingDTO data)
        {
            return _intgf.viewrecordspopup(data);
        }
        


       // POST api/values
     
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
