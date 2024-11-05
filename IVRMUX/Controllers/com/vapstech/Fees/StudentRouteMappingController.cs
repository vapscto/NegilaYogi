using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Fees;
using corewebapi18072016.Delegates.com.vapstech.Fees;
using PreadmissionDTOs.com.vaps.admission;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    public class StudentRouteMappingController : Controller
    {

        StudentRouteMappingDelegate feeTrailAuditreport = new StudentRouteMappingDelegate();
       
        [HttpGet]
        [Route("getalldetails123/{id:int}")]
        public StudentRouteMappingDTO Get123(int id)
        {
            StudentRouteMappingDTO data = new StudentRouteMappingDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            return feeTrailAuditreport.getdata123(data);
        }
       
        //  POST api/values

        [HttpPost]
        [Route("getreport")]
        public StudentRouteMappingDTO getreport([FromBody]StudentRouteMappingDTO data123)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data123.MI_Id = mid;
            return feeTrailAuditreport.getreport(data123);
        }


        [HttpPost]
        [Route("getreportedit")]
        public StudentRouteMappingDTO getreportedit([FromBody]StudentRouteMappingDTO data123)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data123.MI_Id = mid;
            return feeTrailAuditreport.getreportedit(data123);
        }


        [HttpPost]
        [Route("get_cls_secs")]
        public StudentRouteMappingDTO get_cls_secs([FromBody] StudentRouteMappingDTO data123)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data123.MI_Id = mid;
            return feeTrailAuditreport.get_cls_secs(data123);
        }

        [HttpPost]
        [Route("on_pic_route_change")]
        public StudentRouteMappingDTO on_pic_route_change([FromBody] StudentRouteMappingDTO data123)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data123.MI_Id = mid;
            return feeTrailAuditreport.on_pic_route_change(data123);
        }
        [HttpPost]
        [Route("on_drp_route_change")]
        public StudentRouteMappingDTO on_drp_route_change([FromBody] StudentRouteMappingDTO data123)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data123.MI_Id = mid;
            return feeTrailAuditreport.on_drp_route_change(data123);
        }

      

        [HttpPost]
        [Route("savedata")]
        public StudentRouteMappingDTO getclassstudentlist([FromBody]StudentRouteMappingDTO student)
        {
            student.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return feeTrailAuditreport.getlisttwo(student);
        }


        [HttpPost]
        [Route("get_sections")]
        public StudentRouteMappingDTO get_sections([FromBody] StudentRouteMappingDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;         
            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //value.ASMAY_Id = ASMAY_Id;

            return feeTrailAuditreport.get_sections(value);
        }

       
        [Route("deactivate")]
        public StudentRouteMappingDTO deactivate([FromBody] StudentRouteMappingDTO value)
        {
            return feeTrailAuditreport.deactivate(value);

        }
        [HttpPost]
        [Route("searching")]
        public StudentRouteMappingDTO searching([FromBody] StudentRouteMappingDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;
            return feeTrailAuditreport.searching(data);
        }

        [Route("get_loca_sches")]
        public StudentRouteMappingDTO get_loca_sches([FromBody] StudentRouteMappingDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;
            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //value.ASMAY_Id = ASMAY_Id;

            return feeTrailAuditreport.get_loca_sches(value);
        }

        [Route("viewrecordspopup")]
        public StudentRouteMappingDTO viewrecordspopup([FromBody] StudentRouteMappingDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;
            return feeTrailAuditreport.viewrecordspopup(value);
        }
        [Route("SearchByColumn")]
        public StudentRouteMappingDTO SearchByColumn([FromBody] StudentRouteMappingDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;
            return feeTrailAuditreport.SearchByColumn(value);
        }
        [Route("checkduplicateno")]
        public StudentRouteMappingDTO checkduplicateno([FromBody] StudentRouteMappingDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;
            return feeTrailAuditreport.checkduplicateno(value);
        }
        

    }
}
