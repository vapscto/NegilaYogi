using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Sports;
using PreadmissionDTOs.com.vaps.Sports;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class YearEndReportController : Controller
    {
        YearEndReportDelegate deleg = new YearEndReportDelegate();
        [Route("loadDrpDwn/{id:int}")]
        public YearEndReportDTO loadDrpDwn(int id)
        {
            YearEndReportDTO dto = new YearEndReportDTO();
            dto.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return deleg.loadDrpDwn(dto);
        }
        [Route("getReport")]
        public YearEndReportDTO getReport([FromBody]YearEndReportDTO data)
        {
            data.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return deleg.getReport(data);
        }
        [Route("getReportGraph")]
        public YearEndReportDTO getReportGraph([FromBody] YearEndReportDTO obj)
        {
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return deleg.getReportGraph(obj);
        }
    }
}
