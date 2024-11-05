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
    public class ClgBookTransactionReportController : Controller
    {


        ClgBookTransactionReportDelegate _objdel = new ClgBookTransactionReportDelegate();


        [Route("getdetails/{id:int}")]
        public CLGBookTransactionDTO getdetails(int id)
        {
            CLGBookTransactionDTO data = new CLGBookTransactionDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getdetails(data);
        }

        [Route("get_report")]
        public CLGBookTransactionDTO get_report([FromBody] CLGBookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.get_report(data);
        }

    }
}
