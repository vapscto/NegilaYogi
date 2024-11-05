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
    public class BookCirculationReportController : Controller
    {
         BookCirculationReportDelegate _objdel = new BookCirculationReportDelegate();


        [Route("getdetails/{id:int}")]
        public BookCirculationReportDTO getdetails(int id)
        {
            BookCirculationReportDTO data = new BookCirculationReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getdetails(data);
        }


        [Route("getstuddetails")] 
        public BookCirculationReportDTO getstuddetails([FromBody] BookCirculationReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.getstuddetails(data);
        }

        [Route("get_report")]
        public BookCirculationReportDTO get_report([FromBody] BookCirculationReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.get_report(data);
        }
    }
}
