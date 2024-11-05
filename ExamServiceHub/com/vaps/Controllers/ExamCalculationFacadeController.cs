
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
    public class ExamCalculationFacadeController : Controller
    {
        public ExamCalculationInterface _CumulativeReport;

        public ExamCalculationFacadeController(ExamCalculationInterface data)
        {

            _CumulativeReport = data;
        }


        [Route("Getdetails")]
        public ExamReportDTO Getdetails([FromBody]ExamReportDTO data)//int IVRMM_Id
        {
           
            return _CumulativeReport.Getdetails(data);
           
        }
        [HttpPost]
        [Route("get_cls_sections")]
        public ExamReportDTO get_cls_sections([FromBody] ExamReportDTO org)
        {
            // id = 12;
            return _CumulativeReport.get_cls_sections(org);
        }

         [Route("Calculation")]
        public ExamReportDTO Calculation([FromBody] ExamReportDTO org)
        {
            // id = 12;
            return _CumulativeReport.Calculation(org);
        }

        // Marks Approved Process To Display In Portals 
        [Route("getexam")]
        public ExamReportDTO getexam([FromBody] ExamReportDTO org)
        {
            return _CumulativeReport.getexam(org);
        }
        [Route("getclass")]
        public ExamReportDTO getclass([FromBody] ExamReportDTO org)
        {
            return _CumulativeReport.getclass(org);
        }
        [Route("saveapprove")]
        public ExamReportDTO saveapprove([FromBody] ExamReportDTO org)
        {
            return _CumulativeReport.saveapprove(org);
        }

    }
}
