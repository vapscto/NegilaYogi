
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
//using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class HHSMIDFINALCumReportFacadeController : Controller
    {
        public HHSMIDFINALCumReportInterface _CumulativeReport;

        public HHSMIDFINALCumReportFacadeController(HHSMIDFINALCumReportInterface data)
        {
            _CumulativeReport = data;
        }

        [Route("Getdetails")]
        public HHSMIDFINALCumReportDTO Getdetails([FromBody]HHSMIDFINALCumReportDTO data)//int IVRMM_Id
        {           
            return _CumulativeReport.Getdetails(data);           
        }
        
        [Route("validateordernumber")]
        public HHSMIDFINALCumReportDTO validateordernumber([FromBody] HHSMIDFINALCumReportDTO data)
        {
            return _CumulativeReport.validateordernumber(data);
        }

        [Route("savedetails")]
        public async Task<HHSMIDFINALCumReportDTO> savedetails([FromBody] HHSMIDFINALCumReportDTO data)
        {
            return await _CumulativeReport.savedetails(data);
        }
        [Route("savedetailsnew")]
        public async Task<HHSMIDFINALCumReportDTO> savedetailsnew([FromBody] HHSMIDFINALCumReportDTO data)
        {
            return await _CumulativeReport.savedetailsnew(data);
        }

        [Route("cumulativereport")]
        public HHSMIDFINALCumReportDTO cumulativereport([FromBody] HHSMIDFINALCumReportDTO data)
        {
            return _CumulativeReport.cumulativereport(data);
        }
        [Route("ExamSubExamCumulativeReport")]
        public HHSMIDFINALCumReportDTO ExamSubExamCumulativeReport([FromBody] HHSMIDFINALCumReportDTO data)
        {
            return _CumulativeReport.ExamSubExamCumulativeReport(data);
        }
    }
}
