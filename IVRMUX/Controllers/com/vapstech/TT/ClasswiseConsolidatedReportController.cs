using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.TT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.TT
{
    [Route("api/[controller]")]
    public class ClasswiseConsolidatedReportController : Controller
    {

        ClasswiseConsolidatedReportDelegate del = new ClasswiseConsolidatedReportDelegate();

        

        [HttpGet]
        [Route("loaddata")]
        public TT_ClasswiseConsolidatedReportDTO Get([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getalldetails(id);
        }
        [Route("Report")]
public TT_ClasswiseConsolidatedReportDTO Report([FromBody] TT_ClasswiseConsolidatedReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.Report(data);
        }


        [Route("getabreport")]
        public TT_ClasswiseConsolidatedReportDTO getabreport([FromBody] TT_ClasswiseConsolidatedReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getabreport(data);
        }


    }
}
