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
    public class ClgMasterBranchFacadeController : Controller
    {
        public ClgMasterBranchInterface _branchint;

        public ClgMasterBranchFacadeController(ClgMasterBranchInterface branchintf)
        {
            _branchint = branchintf;
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
        public ClgMasterBranchDTO getalldetails([FromBody] ClgMasterBranchDTO data)
        {
            return _branchint.getalldetails(data);
        }
        [Route("savebranch")]
        public ClgMasterBranchDTO savebranch([FromBody] ClgMasterBranchDTO data)
        {
            return _branchint.savebranch(data);
        }
        [Route("editbranch")]
        public ClgMasterBranchDTO editbranch([FromBody] ClgMasterBranchDTO data)
        {
            return _branchint.editbranch(data);
        }
        [Route("activedeactivebranch")]
        public ClgMasterBranchDTO activedeactivebranch([FromBody] ClgMasterBranchDTO data)
        {
            return _branchint.activedeactivebranch(data);
        }

        [Route("saveorder")]
        public ClgMasterBranchDTO saveorder([FromBody] ClgMasterBranchDTO data)
        {
            return _branchint.saveorder(data);
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
