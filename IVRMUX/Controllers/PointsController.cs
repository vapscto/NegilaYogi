using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class PointsController : Controller
    {
        PointsDelegate SCR = new PointsDelegate();
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
        [Route("getdetails/{id:int}")]
        public PointsDTO getdetails(int id)
        {
            PointsDTO data = new PointsDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.mi_id = mid;
            return SCR.getdeatils(data);
        }
        [Route("Getreportdetails")]
        public PointsDTO Getreportdetails([FromBody]PointsDTO rep)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            rep.mi_id = mid;

            return SCR.Getreportdetails(rep);
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        //student health certificate
        [Route("savedata")]
        public PointsDTO savedata([FromBody] PointsDTO maxmin)
        {
            maxmin.mi_id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        
            return SCR.savedata(maxmin);
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
