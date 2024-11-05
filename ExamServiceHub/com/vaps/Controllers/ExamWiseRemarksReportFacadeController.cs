using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ExamWiseRemarksReportFacadeController : Controller
    {
        public ExamWiseRemarksReportInterface _interface;
        public ExamWiseRemarksReportFacadeController(ExamWiseRemarksReportInterface _inter)
        {
            _interface = _inter;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("LoadData")]
        public ExamWiseRemarksReportDTO LoadData([FromBody] ExamWiseRemarksReportDTO data)
        { 
            return _interface.LoadData(data);
        }

        [Route("OnChangeYear")]
        public ExamWiseRemarksReportDTO OnChangeYear([FromBody] ExamWiseRemarksReportDTO data)
        { 
            return _interface.OnChangeYear(data);
        }

        [Route("OnChangeClass")]
        public ExamWiseRemarksReportDTO OnChangeClass([FromBody] ExamWiseRemarksReportDTO data)
        {
            
            return _interface.OnChangeClass(data);
        }

        [Route("OnChangeSection")]
        public ExamWiseRemarksReportDTO OnChangeSection([FromBody] ExamWiseRemarksReportDTO data)
        { 
            return _interface.OnChangeSection(data);
        }

        [Route("OnChangeExam")]
        public ExamWiseRemarksReportDTO OnChangeExam([FromBody] ExamWiseRemarksReportDTO data)
        { 
            return _interface.OnChangeExam(data);
        }

        [Route("GetExamWiseRemarksReport")]
        public ExamWiseRemarksReportDTO GetExamWiseRemarksReport([FromBody] ExamWiseRemarksReportDTO data)
        { 
            return _interface.GetExamWiseRemarksReport(data);
        }

        [Route("GetExamSubjectWiseRemarks_PTReport")]
        public ExamWiseRemarksReportDTO GetExamSubjectWiseRemarks_PTReport([FromBody] ExamWiseRemarksReportDTO data)
        { 
            return _interface.GetExamSubjectWiseRemarks_PTReport(data);
        }

        [Route("GetTermWiseRemarksReport")]
        public ExamWiseRemarksReportDTO GetTermWiseRemarksReport([FromBody] ExamWiseRemarksReportDTO data)
        { 
            return _interface.GetTermWiseRemarksReport(data);
        }
    }
}
