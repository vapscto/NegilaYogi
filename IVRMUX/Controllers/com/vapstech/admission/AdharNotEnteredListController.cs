using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.admission
{
    [Route("api/[controller]")]
    public class AdharNotEnteredList : Controller
    {
        AdhaarNotEnteredListDelegate attdel = new AdhaarNotEnteredListDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [Route("getdetails")]
        public AdhaarNotEnteredListDTO getdetails()
        {
            AdhaarNotEnteredListDTO data = new AdhaarNotEnteredListDTO();
            //    int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return attdel.getInitailData(data);
        }
       // /{id:int }
    
    [Route("getInitailyear")]
        public ClassChangeDTO getInitailyear()
        {
            ClassChangeDTO data = new ClassChangeDTO();
            //    int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           // data.MI_Id = 6;

            //data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return attdel.getInitailyear(data);
        }

        // POST api/values
        [HttpPost]
        [Route("getAttendetails")]
        public AdhaarNotEnteredListDTO getAttendetails([FromBody] AdhaarNotEnteredListDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
         
        
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           
        
            return attdel.getserdata(data);
        }
        [Route("getsection")]
        public AdhaarNotEnteredListDTO getsection([FromBody] AdhaarNotEnteredListDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          
            return attdel.getsection(data);
        }

        [Route("getclass")]
        public AdhaarNotEnteredListDTO getclass([FromBody] AdhaarNotEnteredListDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          
            return attdel.getclass(data);
        }
 

        //classchange
       
    

        [HttpPost]
        [Route("getdetailsclass")]
        public ClassChangeDTO getdetailsclass([FromBody] ClassChangeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return attdel.searchdataclass(data);
        }

        //not promoted list

        [HttpPost]
        [Route("gestudetails")]
        public AdhaarNotEnteredListDTO getstudetails([FromBody] AdhaarNotEnteredListDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return attdel.getstudents(data);
        }

        [HttpPost]
        [Route("Getcountrystatedata")]
        public AdhaarNotEnteredListDTO Getcountrystatedata([FromBody] AdhaarNotEnteredListDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return attdel.Getcountrystatedata(data);
        }
        [Route("getEntryType")]
        public AdhaarNotEnteredListDTO getEntryType([FromBody] AdhaarNotEnteredListDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return attdel.getEntryType(data);
        }
        [Route("getsectionlist")]
        public AdhaarNotEnteredListDTO getsectionlist([FromBody] AdhaarNotEnteredListDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return attdel.getsectionlist(data);
        }
        [Route("getAttendencenotDoneReport")]
        public AdhaarNotEnteredListDTO getAttendencenotDoneReport([FromBody] AdhaarNotEnteredListDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return attdel.getAttendencenotDoneReport(data);
        }
        [Route("getClassEntryType")]
        public AdhaarNotEnteredListDTO getClassEntryType([FromBody] AdhaarNotEnteredListDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return attdel.getClassEntryType(data);
        }

        [HttpPost]
        [Route("emailsend")]
        public AdhaarNotEnteredListDTO emailsend([FromBody] AdhaarNotEnteredListDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return attdel.emailsend(data);
        }
    }
}
