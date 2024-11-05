using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs.com.vaps.TT;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.TT.College;


namespace IVRMUX.Controllers.com.vapstech.TT.College
{
    [Route("api/[controller]")]
    public class CLGPRDDistributionController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        CLGPRDDistributionDelegate ad = new CLGPRDDistributionDelegate();

        // GET: api/Academic/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public CLGPRDDistributionDTO Get([FromQuery] int id)
        {
            CLGPRDDistributionDTO data = new CLGPRDDistributionDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getalldetails(data);
        }
        [HttpGet]
        [Route("viewperiods/{id:int}")]
        public CLGPRDDistributionDTO viewperiods(int id)
        {
            CLGPRDDistributionDTO data = new CLGPRDDistributionDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.TTFPD_Id = id;
            return ad.viewperiods(data);
        }

        // POST: api/Academic
        [HttpPost]
        [Route("getBranch")]
        public CLGPRDDistributionDTO getBranch([FromBody] CLGPRDDistributionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getBranch(data);
        }
        [HttpPost]
        [Route("savedetail")]
        public CLGPRDDistributionDTO savedetail([FromBody] CLGPRDDistributionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.savedetail(data);
        }

        [HttpPost]
        [Route("editprddestr")]
        public CLGPRDDistributionDTO editprddestr([FromBody] CLGPRDDistributionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.editprddestr(data);
        }

        [HttpPost]
        [Route("deactivate")]
        public CLGPRDDistributionDTO deactivate([FromBody] CLGPRDDistributionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.deactivate(data);
        }
      
         [HttpPost]
        [Route("deactivecrsday")]
        public CLGPRDDistributionDTO deactivecrsday([FromBody] CLGPRDDistributionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.deactivecrsday(data);
        }
      

      


    }
}
