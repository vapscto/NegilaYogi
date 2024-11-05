using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Reports.Medical;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Reports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Reports.Medical
{
    [Route("api/[controller]")]
    public class Medical_Criteria1ReportsController : Controller
    {
        public Medical_Criteria1ReportsDelegate objdel = new Medical_Criteria1ReportsDelegate();

        [Route("getdata/{id:int}")]
        public Medical_Criteria1Reports_DTO getdata(int id)
        {
            Medical_Criteria1Reports_DTO data = new Medical_Criteria1Reports_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return objdel.getdata(data);
        }

        [Route("get_report_MC_112")]
        public Medical_Criteria1Reports_DTO get_report_MC_112([FromBody]Medical_Criteria1Reports_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.get_report_MC_112(data);
        }

        [Route("report_MC_141")]
        public Medical_Criteria1Reports_DTO report_MC_141([FromBody]Medical_Criteria1Reports_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.report_MC_141(data);
        }

        [Route("report_MC_142")]
        public Medical_Criteria1Reports_DTO report_MC_142([FromBody]Medical_Criteria1Reports_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.report_MC_142(data);
        }

        [Route("M_IDC121_Report")]
        public Medical_Criteria1Reports_DTO M_IDC121_Report([FromBody]Medical_Criteria1Reports_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.M_IDC121_Report(data);
        }

        [Route("M_SRC122_Report")]
        public Medical_Criteria1Reports_DTO M_SRC122_Report([FromBody]Medical_Criteria1Reports_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.M_SRC122_Report(data);
        }

        [Route("MC_VAC_report_132")]
        public Medical_Criteria1Reports_DTO MC_VAC_report_132([FromBody]Medical_Criteria1Reports_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_VAC_report_132(data);
        }

        [Route("StudentsEnrolledInVAC133_report")]
        public Medical_Criteria1Reports_DTO StudentsEnrolledInVAC133_report([FromBody]Medical_Criteria1Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.StudentsEnrolledInVAC133_report(data);
        }

        [Route("MC_StudentUTFV_134_Report")]
        public Medical_Criteria1Reports_DTO MC_StudentUTFV_134_Report([FromBody]Medical_Criteria1Reports_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.MC_StudentUTFV_134_Report(data);
        }
        
    }
}
