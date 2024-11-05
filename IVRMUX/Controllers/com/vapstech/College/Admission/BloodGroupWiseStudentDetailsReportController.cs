using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class BloodGroupWiseStudentDetailsReportController : Controller
    {
        BloodGroupWiseStudentDetailsReportDelegate del = new BloodGroupWiseStudentDetailsReportDelegate();

        [Route("loaddata/{id:int}")]
        public BloodGroupWiseStudentDetailsReportDTO loaddata(int id)
        {
            BloodGroupWiseStudentDetailsReportDTO data = new BloodGroupWiseStudentDetailsReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loaddata(data);
        }
        [Route("getcourse")]
        public BloodGroupWiseStudentDetailsReportDTO getcourse([FromBody] BloodGroupWiseStudentDetailsReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getcourse(data);
        }
        [Route("getbranch")]
        public BloodGroupWiseStudentDetailsReportDTO getbranch([FromBody] BloodGroupWiseStudentDetailsReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getbranch(data);
        }
        [Route("getsemester")]
        public BloodGroupWiseStudentDetailsReportDTO getsemester([FromBody] BloodGroupWiseStudentDetailsReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getsemester(data);
        }
        [Route("Report")]
        public BloodGroupWiseStudentDetailsReportDTO Report([FromBody] BloodGroupWiseStudentDetailsReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.Report(data);
        }
    }
}
