using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.PettyCash.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
using PreadmissionDTOs.com.vaps.VMS.Training;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SummaryreportsFacadeController : Controller
    {
        public SummaryreportsInterface _interface;

        public SummaryreportsFacadeController(SummaryreportsInterface _inter)
        {
            _interface = _inter;
        }
        [Route("onloaddata")]
        public SummaryreportsDTO onloaddata([FromBody] SummaryreportsDTO data)
        {
            return _interface.onloaddata(data);
        }

        [Route("getreport")]
        public SummaryreportsDTO saverecord([FromBody] SummaryreportsDTO data)
        {
            return _interface.getreport(data);
        }
        [Route("inhouseReportreport")]
        public SummaryreportsDTO inhouseReportreport([FromBody] SummaryreportsDTO data)
        {
            return _interface.inhouseReportreport(data);
        }
        [Route("trainingcertificate")]
        public SummaryreportsDTO trainingcertificate([FromBody] SummaryreportsDTO data)
        {
            return _interface.trainingcertificate(data);
        }
    }
}
