using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Admission.Criteria7;
using IVRMUX.Delegates.NAAC.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Admission;
using PreadmissionDTOs.NAAC.Reports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Admission.Criteria7
{
    [Route("api/[controller]")]
    public class NAAC711GenderEquityReportController : Controller
    {
        NAAC711GenderEquityReportDelegate del = new NAAC711GenderEquityReportDelegate();

        [Route("loaddata/{id:int}")]
        public NAACAC7Report_DTO loaddata(int id)
        {
            NAACAC7Report_DTO data = new NAACAC7Report_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }

        [Route("Report")]
        public NAACAC7Report_DTO Report([FromBody]NAACAC7Report_DTO data)
        {
            //NAACAC7Report_DTO data = new NAACAC7Report_DTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.Report(data);
        }
    }
}
