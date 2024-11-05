using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VisitorsManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VisitorsManagement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VisitorsManagement
{
    [Route("api/[controller]")]
    public class GatePassReportController : Controller
    {
        GatePassReportDelegate delobj = new GatePassReportDelegate();
        // GET: api/<controller>
      
        [HttpPost]
        [Route("report")]
        public GatePassReportDTO report([FromBody]GatePassReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delobj.report(data);
        }

        [Route("loaddata/{id:int}")]
        public GatePassReportDTO loaddata(int id)
        {
            GatePassReportDTO dto = new GatePassReportDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.loaddata(dto);
        }

        [Route("reportforMobile")]
        public GatePassReportDTO reportforMobile([FromBody] GatePassReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delobj.reportforMobile(data);
        }

    }
}
