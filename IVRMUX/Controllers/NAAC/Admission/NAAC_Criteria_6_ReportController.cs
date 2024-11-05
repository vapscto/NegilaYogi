using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Admission
{
    [Route("api/[controller]")]
    public class NAAC_Criteria_6_ReportController : Controller
    {
        NAAC_Criteria_6_ReportDelegate del = new NAAC_Criteria_6_ReportDelegate();
        [Route("loaddata1/{id:int}")]
        public NAAC_Criteria_6_DTO loaddata1(int id)
        {
            NAAC_Criteria_6_DTO data = new NAAC_Criteria_6_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.loaddata(data);
        }
        [Route("get_report")]
        public NAAC_Criteria_6_DTO save([FromBody]NAAC_Criteria_6_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.get_report(data);
        }
    
    }
}
