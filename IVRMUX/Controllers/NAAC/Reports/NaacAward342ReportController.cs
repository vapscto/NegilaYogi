using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Reports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Reports
{
    [Route("api/[controller]")]
    public class NaacAward342ReportController : Controller
    {

        NaacAward342ReportDelegate del = new NaacAward342ReportDelegate();

        [Route("getdata/{id:int}")]
        public NaacAward342ReportDTO getdata(int id)
        {

            NaacAward342ReportDTO data = new NaacAward342ReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            return del.getdata(data);
        }


        [Route("get_report")]
        public NaacAward342ReportDTO get_report([FromBody]NaacAward342ReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return del.get_report(data);
        }
    }
}
