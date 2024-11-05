using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.College.Admission;
using PreadmissionDTOs.com.vaps.College.Admission;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class ClgMasterBranchController : Controller
    {

        ClgMasterBranchDelegate _branch = new ClgMasterBranchDelegate();

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
        public ClgMasterBranchDTO getalldetails (int id)
        {
            ClgMasterBranchDTO data = new ClgMasterBranchDTO();
            data.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _branch.getalldetails(data);
        }
        [Route("savebranch")]
        public ClgMasterBranchDTO savebranch([FromBody] ClgMasterBranchDTO data)
        {            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _branch.savebranch(data);
        }

        [Route("editbranch")]
        public ClgMasterBranchDTO editbranch([FromBody] ClgMasterBranchDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _branch.editbranch(data);
        }
        [Route("activedeactivebranch")]
        public ClgMasterBranchDTO activedeactivebranch([FromBody] ClgMasterBranchDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _branch.activedeactivebranch(data);
        }
        [Route("saveorder")]
        public ClgMasterBranchDTO saveorder([FromBody] ClgMasterBranchDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _branch.saveorder(data);
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
