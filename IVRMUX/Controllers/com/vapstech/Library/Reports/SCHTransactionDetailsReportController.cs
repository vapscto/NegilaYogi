using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Library.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Library.Reports
{
    [Route("api/[controller]")]
    public class SCHTransactionDetailsReportController : Controller
    {
        // GET: api/<controller>

       private SCHTransactionDetailsReportDelegate _delobj = new SCHTransactionDetailsReportDelegate();

        [Route("getdetails/{id:int}")]
        public SCHTransactionDetailsReportDTO getdetails(int id)
        {
            SCHTransactionDetailsReportDTO data = new SCHTransactionDetailsReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.getdetails(data);
        }
        [Route("get_report")]
        public SCHTransactionDetailsReportDTO get_report([FromBody]SCHTransactionDetailsReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.get_report(data);
        }

    }
}
