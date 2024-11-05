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
    public class CLGFixingController : Controller
    {

        CLGFixingDelegate ad = new CLGFixingDelegate();

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
        public CLGFixingDTO Get([FromQuery] int id)
        {
            CLGFixingDTO data = new CLGFixingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getalldetails(data);
        }
 
        [Route("edittab1")]
        public CLGFixingDTO editlab([FromBody] CLGFixingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return ad.edittab1(data);
        }

        [HttpPost]
        [Route("savetab1")]
        public CLGFixingDTO savetab1([FromBody] CLGFixingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.savetab1(data);
        }
        [HttpPost]
        [Route("viewrecordspopup")]
        public CLGFixingDTO viewrecordspopup([FromBody] CLGFixingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.viewrecordspopup(data);
        }

        [HttpPost]
        [Route("deactivatetab1")]
        public CLGFixingDTO deactivatetab1([FromBody] CLGFixingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.deactivatetab1(data);
        }

        //TAB1 END FIXING DAY PERIOD
        //TAB2 START FIXING DAY STAFF
        [HttpPost]
        [Route("savetab2")]
        public CLGFixingDTO savetab2([FromBody] CLGFixingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.savetab2(data);
        }
        [HttpPost]
        [Route("viewtab2grid")]
        public CLGFixingDTO viewtab2grid([FromBody] CLGFixingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.viewtab2grid(data);
        }
         [HttpPost]
        [Route("gettab2editdata")]
        public CLGFixingDTO gettab2editdata([FromBody] CLGFixingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.gettab2editdata(data);
        }

        [HttpPost]
        [Route("deactivatetab2")]
        public CLGFixingDTO deactivatetab2([FromBody] CLGFixingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.deactivatetab2(data);
        }
        //TAB2 END FIXING DAY STAFF


        //TAB3 END FIXING DAY SUBJECT
        
        [HttpPost]
        [Route("savetab3")]
        public CLGFixingDTO savetab3([FromBody] CLGFixingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.savetab3(data);
        }

        [HttpPost]
        [Route("viewtab3grid")]
        public CLGFixingDTO viewtab3grid([FromBody] CLGFixingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.viewtab3grid(data);
        }

        [HttpPost]
        [Route("edittab3")]
        public CLGFixingDTO edittab3([FromBody] CLGFixingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.edittab3(data);
        }

        [HttpPost]
        [Route("deactivatetab3")]
        public CLGFixingDTO deactivatetab3([FromBody] CLGFixingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.deactivatetab3(data);
        }

        //TAB3 END FIXING DAY SUBJECT

        //TAB4 START FIXING PERIOD STAFF

        [HttpPost]
        [Route("savetab4")]
        public CLGFixingDTO savetab4([FromBody] CLGFixingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.savetab4(data);
        }

        [HttpPost]
        [Route("viewtab4")]
        public CLGFixingDTO viewtab4([FromBody] CLGFixingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.viewtab4(data);
        }
        [HttpPost]
        [Route("edittab4")]
        public CLGFixingDTO edittab4([FromBody] CLGFixingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.edittab4(data);
        }
        [HttpPost]
        [Route("deactivatetab4")]
        public CLGFixingDTO deactivatetab4([FromBody] CLGFixingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.deactivatetab4(data);
        }

        //TAB4 END FIXING PERIOD STAFF
        //TAB5 START FIXING PERIOD SUBJECT
        
            [HttpPost]
        [Route("savetab5")]
        public CLGFixingDTO savetab5([FromBody] CLGFixingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.savetab5(data);
        }

        [HttpPost]
        [Route("viewtab5")]
        public CLGFixingDTO viewtab5([FromBody] CLGFixingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.viewtab5(data);
        }
        [HttpPost]
        [Route("edittab5")]
        public CLGFixingDTO edittab5([FromBody] CLGFixingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.edittab5(data);
        }[HttpPost]

        [Route("deactivatetab5")]
        public CLGFixingDTO deactivatetab5([FromBody] CLGFixingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.deactivatetab5(data);
        }
        //TAB5 END FIXING PERIOD SUBJECT




    }
}
