using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Fees;
using corewebapi18072016.Delegates.com.vapstech.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    public class Student_SettlementController : Controller
    {
        Student_SettlementDelegate _del = new Student_SettlementDelegate();

        [HttpGet]
        [Route("Getdetails")]
        public Student_SettlementDTO Getdetails(Student_SettlementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _del.Getdetails(data);
        }
       
        [HttpPost]
        [Route("getdates")]
        public Student_SettlementDTO getdates([FromBody] Student_SettlementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _del.getdates(data);
        }
        [Route("savedata")]
        public Student_SettlementDTO savedata([FromBody] Student_SettlementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _del.savedata(data);
        }
        [Route("viewrecords")]
        public Student_SettlementDTO viewrecords([FromBody] Student_SettlementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _del.viewrecords(data);
        }
        [Route("get_classes")]
        public Student_SettlementDTO get_classes([FromBody] Student_SettlementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.get_classes(data);
        }
        [Route("get_sections")]
        public Student_SettlementDTO get_sections([FromBody] Student_SettlementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _del.get_sections(data);
        }
        [Route("get_routes")]
        public Student_SettlementDTO get_routes([FromBody] Student_SettlementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.get_routes(data);
        }
        [Route("getreport")]
        public Student_SettlementDTO getreport([FromBody] Student_SettlementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _del.getreport(data);
        }

        [Route("getreport1")]
        public Student_SettlementDTO getreport1([FromBody] Student_SettlementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _del.getreport1(data);
        }

        [Route("fillmerchants")]
        public Student_SettlementDTO fillmerchants([FromBody] Student_SettlementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _del.fillmerchants(data);
        }

    }
}
