using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.OnlineProgram.Interface;
using PreadmissionDTOs.NAAC.OnlineProgram;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.OnlineProgram.Facade
{
    [Route("api/[controller]")]
    public class OnlineProgramReportFacade : Controller
    {
        public OnlineProgramReportInterface _inter;
        public OnlineProgramReportFacade(OnlineProgramReportInterface a)
        {
            _inter = a;
        }
        [Route("getyearlyprogram")]
        public OnlineProgramReport_DTO getyearlyprogram([FromBody] OnlineProgramReport_DTO data)
        {
            return _inter.getyearlyprogram(data);
        }
        [Route("getYearlyProgramReport")]
        public Task<OnlineProgramReport_DTO> getYearlyProgramReport([FromBody] OnlineProgramReport_DTO data)
        {
            return _inter.getYearlyProgramReport(data);
        }
        [Route("ConferenceDetailsReport")]
        public Task<OnlineProgramReport_DTO> ConferenceDetailsReport([FromBody] OnlineProgramReport_DTO data)
        {
            return _inter.ConferenceDetailsReport(data);
        }
    }
}
