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
    public class ClgyearlycoursemappingController : Controller
    {
        ClgyearlycoursemappingDelegate _yearly = new ClgyearlycoursemappingDelegate();
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
        public ClgyearlycoursemappingDTO getalldetails (int id)
        {
            ClgyearlycoursemappingDTO data = new ClgyearlycoursemappingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _yearly.getalldetails(data);
        }
        [HttpPost]
        [Route("getbranches")]
        public ClgyearlycoursemappingDTO getbranches([FromBody] ClgyearlycoursemappingDTO data)
        {           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _yearly.getbranches(data);
        }
        [Route("getsemisters")]
        public ClgyearlycoursemappingDTO getsemisters([FromBody] ClgyearlycoursemappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _yearly.getsemisters(data);
        }
        [Route("savedata")]
        public ClgyearlycoursemappingDTO savedata([FromBody] ClgyearlycoursemappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _yearly.savedata(data);
        }
        [Route("searchdata")]
        public ClgyearlycoursemappingDTO searchdata([FromBody] ClgyearlycoursemappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _yearly.searchdata(data);
        }
        [Route("viewrecordspopup")]
        public ClgyearlycoursemappingDTO viewrecordspopup([FromBody] ClgyearlycoursemappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _yearly.viewrecordspopup(data);
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
