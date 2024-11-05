using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.OnlineProgram;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.OnlineProgram;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.OnlineProgram
{
    [Route("api/[controller]")]
    public class OnlineProgramReportController : Controller
    {
        OnlineProgramReportDelegate del = new OnlineProgramReportDelegate();
        [Route("getyearlyprogram/{id:int}")]
        public OnlineProgramReport_DTO getyearlyprogram(int id)
        {
            OnlineProgramReport_DTO data = new OnlineProgramReport_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getyearlyprogram(data);
        }
        [Route("getYearlyProgramReport")]
        public OnlineProgramReport_DTO getYearlyProgramReport([FromBody] OnlineProgramReport_DTO data)
        {
            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getYearlyProgramReport(data);
        }
        [Route("ConferenceDetailsReport")]
        public OnlineProgramReport_DTO ConferenceDetailsReport([FromBody] OnlineProgramReport_DTO data)
        {
            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.ConferenceDetailsReport(data);
        }


    }
}
