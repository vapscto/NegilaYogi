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
    public class LostBookReportController : Controller
    {
        LostBookReportDelegate _delobj = new LostBookReportDelegate();
        
        [Route("getdetails/{id:int}")]
        public LostBookReport_DTO getdetails(int id)
        {
            LostBookReport_DTO data = new LostBookReport_DTO();

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _delobj.getdetails(data);
        }

        [Route("get_report")]
        public LostBookReport_DTO get_report([FromBody] LostBookReport_DTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _delobj.get_report(data);
        }




    }
}
