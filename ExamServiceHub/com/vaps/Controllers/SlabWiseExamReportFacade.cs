using ExamServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SlabWiseExamReportFacade : Controller
    {
        public SlabWiseExamReportinterface _slabReport;
        public SlabWiseExamReportFacade(SlabWiseExamReportinterface data)
        {
            _slabReport = data;
        }
        [Route("getsubjects")]
        public  SlabWiseExamReportDTO getsubjects([FromBody] SlabWiseExamReportDTO data)
        {
            return  _slabReport.getsubjects(data);
        }
        [Route("getslabreport")]
        public SlabWiseExamReportDTO getslabreport([FromBody] SlabWiseExamReportDTO data)
        {
            return _slabReport.getslabreport(data);
        }
    }
}
