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
    public class StudentCompliantsController : Controller
    {
        StudentCompliantsDelegate del = new StudentCompliantsDelegate();
        [Route("loaddata/{id:int}")]
        public StudentCompliants_DTO loaddata(int id)
        {
            StudentCompliants_DTO data = new StudentCompliants_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                       
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.loaddata(data);
        }
        [Route("getstudents")]
        public StudentCompliants_DTO getstudents([FromBody] StudentCompliants_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));  
            return del.getstudents(data);
        }
        [Route("edittab1")]
        public StudentCompliants_DTO edittab1([FromBody] StudentCompliants_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));  
            return del.edittab1(data);
        }
        [Route("getstudentdetails")]
        public StudentCompliants_DTO getstudentdetails([FromBody] StudentCompliants_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));  
            return del.getstudentdetails(data);
        }
        [Route("save")]
        public StudentCompliants_DTO save([FromBody] StudentCompliants_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));  
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));  
            return del.save(data);
        }
        [Route("getorganizationdata")]
        public StudentCompliants_DTO getorganizationdata([FromBody] StudentCompliants_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));  
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));  
            return del.getorganizationdata(data);
        }
        [Route("deactive")]
        public StudentCompliants_DTO deactive([FromBody] StudentCompliants_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));  
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));  
            return del.deactive(data);
        }
        [Route("searchfilter")]
        public StudentCompliants_DTO searchfilter([FromBody] StudentCompliants_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));  
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));  
            return del.searchfilter(data);
        }
        [Route("report")]
        public StudentCompliants_DTO report([FromBody] StudentCompliants_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));  
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));  
            return del.report(data);
        }
    }
}
