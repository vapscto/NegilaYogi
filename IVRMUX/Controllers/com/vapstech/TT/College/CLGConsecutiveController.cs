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
    public class CLGConsecutiveController : Controller
    {

        CLGConsecutiveDelegate ad = new CLGConsecutiveDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        
        // GET: api/Academic/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public CLGConsecutiveDTO Get([FromQuery] int id)
        {
            CLGConsecutiveDTO data = new CLGConsecutiveDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getalldetails(data);
        }
 
        [Route("editconv")]
        public CLGConsecutiveDTO editconv([FromBody] CLGConsecutiveDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return ad.editconv(data);
        }

        [HttpPost]
        [Route("savedetail")]
        public CLGConsecutiveDTO savedetail([FromBody] CLGConsecutiveDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.savedetail(data);
        }

        [HttpPost]
        [Route("deactivate")]
        public CLGConsecutiveDTO deactivate([FromBody] CLGConsecutiveDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.deactivate(data);
        }
      
        
      

    }
}
