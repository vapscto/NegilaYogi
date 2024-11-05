using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using SportsServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SportPointAndRecordReportFacade : Controller
    {
        public SportPointAndRecordReportInterface _ReportContext;

        public SportPointAndRecordReportFacade(SportPointAndRecordReportInterface dt)
        {
            _ReportContext = dt;
        }


        [Route("Getdetails")]
        public SportStudentParticipationReportDTO Getdetails([FromBody]SportStudentParticipationReportDTO data)//int IVRMM_Id
        {

            return _ReportContext.Getdetails(data);
        }


        [Route("showdetails")]
        public SportStudentParticipationReportDTO showdetails([FromBody] SportStudentParticipationReportDTO data)
        {
            return _ReportContext.showdetails(data);
        }
    }
}
