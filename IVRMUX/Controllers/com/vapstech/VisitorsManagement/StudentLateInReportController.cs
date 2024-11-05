using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VisitorsManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VisitorsManagement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VisitorsManagement
{
    [Route("api/[controller]")]
    public class StudentLateInReportController : Controller
    {

        StudentLateInReportDelegate deleg = new StudentLateInReportDelegate();


        [Route("loaddata/{id:int}")]
        public LateInStudent_DTO loaddata(int id)
        {
            LateInStudent_DTO dto = new LateInStudent_DTO();
            dto.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return deleg.loaddata(dto);
        }
        [Route("getReport")]
        public LateInStudent_DTO getReport([FromBody]LateInStudent_DTO data)
        {
            data.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return deleg.getReport(data);
        }
       

       
    }
}
