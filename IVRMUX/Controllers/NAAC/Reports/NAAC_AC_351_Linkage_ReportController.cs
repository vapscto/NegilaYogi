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
    public class NAAC_AC_351_Linkage_ReportController : Controller
    {
        NAAC_AC_351_Linkage_ReportDelegates del = new NAAC_AC_351_Linkage_ReportDelegates();
        // GET: api/<controller>
        [Route("loaddata/{id:int}")]
        public NAAC_AC_351_Linkage_ReportDTO loaddata(int id)
        {
            NAAC_AC_351_Linkage_ReportDTO data = new NAAC_AC_351_Linkage_ReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }
        [Route("report")]
        public NAAC_AC_351_Linkage_ReportDTO report([FromBody]NAAC_AC_351_Linkage_ReportDTO data)
        {
            //NaacCriteria4ReportDTO data = new NaacCriteria4ReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.report(data);
        }        
    }
}
