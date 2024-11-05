using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeExamServiceHub.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeExamServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class CollegeExamGeneralReportFacadeController : Controller
    {
        public CollegeExamGeneralReportInterface _interface;

        public CollegeExamGeneralReportFacadeController(CollegeExamGeneralReportInterface _inter)
        {
            _interface = _inter;
        }

        [Route("MasterGradeReportLoadData")]
        public CollegeExamGeneralReportDTO MasterGradeReportLoadData([FromBody] CollegeExamGeneralReportDTO data)
        {
            return _interface.MasterGradeReportLoadData(data);
        }

        [Route("MasterGradeReportDetails")]
        public CollegeExamGeneralReportDTO MasterGradeReportDetails([FromBody] CollegeExamGeneralReportDTO data)
        {
            return _interface.MasterGradeReportDetails(data);
        }
    }
}
