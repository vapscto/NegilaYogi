using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMSServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SMSemailStaffReportFacade : Controller
    {
        public SMSemailStaffReportInterface _ads;

        public SMSemailStaffReportFacade(SMSemailStaffReportInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public SMSemailStaffReportDTO getinitialdata([FromBody]SMSemailStaffReportDTO dto)
        {
            return _ads.getBasicData(dto);
        }      
        [Route("getreport")]
        public SMSemailStaffReportDTO getreport([FromBody]SMSemailStaffReportDTO dto)
        {
            return _ads.getreport(dto);
        }
        [Route("smsemail")]
        public SMSemailStaffReportDTO smsemail([FromBody]SMSemailStaffReportDTO dto)
        {
            return _ads.smsemail(dto);
        }
        //Destination
        [Route("Destination")]
        public SMSemailStaffReportDTO Destination([FromBody]SMSemailStaffReportDTO dto)
        {
            return _ads.Destination(dto);
        }
    }
}
