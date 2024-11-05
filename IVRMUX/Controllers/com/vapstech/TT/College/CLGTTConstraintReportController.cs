using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.TT.College;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.TT.College
{
    [Route("api/[controller]")]
    public class CLGTTConstraintReportController : Controller
    {
        CLGTTConstraintReportDelegate objdelegate = new CLGTTConstraintReportDelegate();

        [HttpGet]
        [Route("getalldetails")]
        public CLGTTConstraintReportDTO Get([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getalldetails(id);
        }
        [HttpPost]
        [Route("getpagedetails")]
        public CLGTTConstraintReportDTO getpagedetails([FromBody] CLGTTConstraintReportDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getpagedetails(data);

        }

    }
}
