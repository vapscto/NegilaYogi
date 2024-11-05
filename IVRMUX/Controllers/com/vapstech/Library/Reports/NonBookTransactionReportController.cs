using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Library.Reports
{
    [Route("api/[controller]")]
    public class NonBookTransactionReportController : Controller
    {

        NonBookTransactionReportDelegate _objdel = new NonBookTransactionReportDelegate();


        [Route("getdetails/{id:int}")]
        public NonBookReport_DTO getdetails(int id)
        {
            NonBookReport_DTO data = new NonBookReport_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getdetails(data);
        }
        
        [Route("get_report")]
        public NonBookReport_DTO get_report([FromBody] NonBookReport_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return _objdel.get_report(data);
        }



    }
}
