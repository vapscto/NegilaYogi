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
    public class BookRegisterReportController : Controller
    {
        BookRegisterReportDelegate _objdel = new BookRegisterReportDelegate();


        [Route("getdetails/{id:int}")]
        public BookRegisterReportDTO getdetails(int id)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            id = mi_id;
            return _objdel.getdetails(id);
        }
        [Route("get_report")]
        public BookRegisterReportDTO get_report([FromBody]BookRegisterReportDTO data)
        {
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.get_report(data);
        }
        //BarCode
        [Route("BarCode")]
        public BookRegisterReportDTO BarCode([FromBody]BookRegisterReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.BarCode(data);
        }
    }
}
