using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Hostel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.Hostel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Hostel
{
    [Route("api/[controller]")]
    public class Hostel_Student_InOutReportController : Controller
    {
       
        Hostel_Student_InOutReportDelegate _delg = new Hostel_Student_InOutReportDelegate();
        
        [HttpGet]  
        [Route("getalldetails/{id:int}")]
        public Hostel_Student_InOutDTO getdetails(int id)
        {
            Hostel_Student_InOutDTO data = new Hostel_Student_InOutDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getdetails(data);
        }


        [Route("empname")]
        public Hostel_Student_InOutDTO empname([FromBody]Hostel_Student_InOutDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.empname(data);
        }

        [Route("savedetail")]
        public Hostel_Student_InOutDTO savedetail([FromBody]Hostel_Student_InOutDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.savedetail(data);
        }
        [Route("deletedetails")]
        public Hostel_Student_InOutDTO deletedetails([FromBody]Hostel_Student_InOutDTO data)
        {            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.deleterec(data);
        }


        //report


        [HttpGet]
        [Route("loaddata/{id:int}")]
        public Hostel_Student_InOutDTO loaddata(int id)
        {
            Hostel_Student_InOutDTO data = new Hostel_Student_InOutDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.loaddata(data);
        }



        [Route("report")]
            public Hostel_Student_InOutDTO report([FromBody] Hostel_Student_InOutDTO data)
            {
                data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
                data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
                return _delg.report(data);
            }
    }
}

