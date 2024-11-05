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
    public class CLGRestrictionController : Controller
    {

        CLGRestrictionDelegate ad = new CLGRestrictionDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        

        //TAB1 START FIXING DAY PERIOD
        // GET: api/Academic/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public CLGRestrictionDTO Get([FromQuery] int id)
        {
            CLGRestrictionDTO data = new CLGRestrictionDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getalldetails(data);
        }
 
        [Route("edittab1")]
        public CLGRestrictionDTO editlab([FromBody] CLGRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return ad.edittab1(data);
        }

        [HttpPost]
        [Route("savetab1")]
        public CLGRestrictionDTO savetab1([FromBody] CLGRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.savetab1(data);
        }
        [HttpPost]
        [Route("viewrecordspopup")]
        public CLGRestrictionDTO viewrecordspopup([FromBody] CLGRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.viewrecordspopup(data);
        }

        [HttpPost]
        [Route("deactivatetab1")]
        public CLGRestrictionDTO deactivatetab1([FromBody] CLGRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.deactivatetab1(data);
        }

        //TAB1 END FIXING DAY PERIOD
        //TAB2 START FIXING DAY STAFF
        [HttpPost]
        [Route("savetab2")]
        public CLGRestrictionDTO savetab2([FromBody] CLGRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.savetab2(data);
        }
        [HttpPost]
        [Route("viewtab2grid")]
        public CLGRestrictionDTO viewtab2grid([FromBody] CLGRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.viewtab2grid(data);
        }
         [HttpPost]
        [Route("gettab2editdata")]
        public CLGRestrictionDTO gettab2editdata([FromBody] CLGRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.gettab2editdata(data);
        }

        [HttpPost]
        [Route("deactivatetab2")]
        public CLGRestrictionDTO deactivatetab2([FromBody] CLGRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.deactivatetab2(data);
        }
        //TAB2 END FIXING DAY STAFF


        //TAB3 END FIXING DAY SUBJECT
        
        [HttpPost]
        [Route("savetab3")]
        public CLGRestrictionDTO savetab3([FromBody] CLGRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.savetab3(data);
        }

        [HttpPost]
        [Route("viewtab3grid")]
        public CLGRestrictionDTO viewtab3grid([FromBody] CLGRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.viewtab3grid(data);
        }

        [HttpPost]
        [Route("edittab3")]
        public CLGRestrictionDTO edittab3([FromBody] CLGRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.edittab3(data);
        }

        [HttpPost]
        [Route("deactivatetab3")]
        public CLGRestrictionDTO deactivatetab3([FromBody] CLGRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.deactivatetab3(data);
        }

        //TAB3 END FIXING DAY SUBJECT

        //TAB4 START FIXING PERIOD STAFF

        [HttpPost]
        [Route("savetab4")]
        public CLGRestrictionDTO savetab4([FromBody] CLGRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.savetab4(data);
        }

        [HttpPost]
        [Route("viewtab4")]
        public CLGRestrictionDTO viewtab4([FromBody] CLGRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.viewtab4(data);
        }
        [HttpPost]
        [Route("edittab4")]
        public CLGRestrictionDTO edittab4([FromBody] CLGRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.edittab4(data);
        }
        [HttpPost]
        [Route("deactivatetab4")]
        public CLGRestrictionDTO deactivatetab4([FromBody] CLGRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.deactivatetab4(data);
        }

        //TAB4 END FIXING PERIOD STAFF
        //TAB5 START FIXING PERIOD SUBJECT
        
            [HttpPost]
        [Route("savetab5")]
        public CLGRestrictionDTO savetab5([FromBody] CLGRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.savetab5(data);
        }

        [HttpPost]
        [Route("viewtab5")]
        public CLGRestrictionDTO viewtab5([FromBody] CLGRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.viewtab5(data);
        }
        [HttpPost]
        [Route("edittab5")]
        public CLGRestrictionDTO edittab5([FromBody] CLGRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.edittab5(data);
        }[HttpPost]

        [Route("deactivatetab5")]
        public CLGRestrictionDTO deactivatetab5([FromBody] CLGRestrictionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.deactivatetab5(data);
        }
        //TAB5 END FIXING PERIOD SUBJECT




    }
}
