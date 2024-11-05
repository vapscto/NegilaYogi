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
    public class NaacMOU352ReportController : Controller
    {


        NaacMOU352ReportDelegate del = new NaacMOU352ReportDelegate();

        [Route("getdata/{id:int}")]
        public NaacMOU352ReportDTO getdata(int id)
        {

            NaacMOU352ReportDTO data = new NaacMOU352ReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            return del.getdata(data);
        }


        [Route("get_report")]
        public NaacMOU352ReportDTO get_report([FromBody]NaacMOU352ReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return del.get_report(data);
        }

    }
}
