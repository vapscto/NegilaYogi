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
    public class ChangeOfBranchReportController : Controller
    {
        ChangeOfBranchReportDelegate del = new ChangeOfBranchReportDelegate();
        [Route("loaddata/{id:int}")]
        public ChangeOfBranchReportDTO loaddata(int id)
        {
            ChangeOfBranchReportDTO data = new ChangeOfBranchReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loaddata(data);
        }
        [Route("getcourse")]
        public ChangeOfBranchReportDTO getcourse([FromBody] ChangeOfBranchReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getcourse(data);
        }
        [Route("getbranch")]
        public ChangeOfBranchReportDTO getbranch([FromBody] ChangeOfBranchReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getbranch(data);
        }
        [Route("Report")]
        public ChangeOfBranchReportDTO Report([FromBody] ChangeOfBranchReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.Report(data);
        }

    }
}
