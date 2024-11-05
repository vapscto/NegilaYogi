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
    public class CLGLabController : Controller
    {

        CLGLabDelegate ad = new CLGLabDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        
        // GET: api/Academic/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public CLGLabDTO Get([FromQuery] int id)
        {
            CLGLabDTO data = new CLGLabDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getalldetails(data);
        }
 
        [Route("editlab")]
        public CLGLabDTO editlab([FromBody] CLGLabDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return ad.editlab(data);
        }

        [HttpPost]
        [Route("savedetail")]
        public CLGLabDTO savedetail([FromBody] CLGLabDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.savedetail(data);
        }
        [HttpPost]
        [Route("viewrecordspopup")]
        public CLGLabDTO viewrecordspopup([FromBody] CLGLabDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.viewrecordspopup(data);
        }

        [HttpPost]
        [Route("deactivate")]
        public CLGLabDTO deactivate([FromBody] CLGLabDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.deactivate(data);
        }
      
        
      

    }
}
