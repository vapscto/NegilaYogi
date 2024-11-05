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
    public class NaacCriteria4ReportController : Controller
    {
        NaacCriteria4ReportDelegate del = new NaacCriteria4ReportDelegate();

        [Route("loaddata/{id:int}")]
        public NaacCriteria4ReportDTO loaddata(int id)
        {
            NaacCriteria4ReportDTO data = new NaacCriteria4ReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }


        [Route("Report")]
        public NaacCriteria4ReportDTO Report([FromBody]NaacCriteria4ReportDTO data)
        {
            //NaacCriteria4ReportDTO data = new NaacCriteria4ReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.Report(data);
        }
        [Route("ReportCriteria4")]
        public NaacCriteria4ReportDTO ReportCriteria4([FromBody]NaacCriteria4ReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.ReportCriteria4(data);
        }
       

        [Route("ExpAcaReport")]
        public NaacCriteria4ReportDTO ExpAcaReport([FromBody]NaacCriteria4ReportDTO data)
        {
            //NaacCriteria4ReportDTO data = new NaacCriteria4ReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.ExpAcaReport(data);
        }
    }
}
