using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class VikasaSchoolExamWiseCumulativeReportFacadeController : Controller
    {
        public VikasaSchoolExamWiseCumulativeReportInterface _ReportContext;

        public VikasaSchoolExamWiseCumulativeReportFacadeController(VikasaSchoolExamWiseCumulativeReportInterface dt)
        {
            _ReportContext = dt;
        }


        [Route("Getdetails")]
        public VikasaSubjectwiseCumulativeReportDTO Getdetails([FromBody]VikasaSubjectwiseCumulativeReportDTO data)//int IVRMM_Id
        {

            return _ReportContext.Getdetails(data);
        }


        [Route("showdetails")]
        public VikasaSubjectwiseCumulativeReportDTO showdetails([FromBody] VikasaSubjectwiseCumulativeReportDTO data)
        {
            return _ReportContext.showdetails(data);
        }

        [Route("get_class")]
        public VikasaSubjectwiseCumulativeReportDTO get_class([FromBody] VikasaSubjectwiseCumulativeReportDTO data)
        {
            return _ReportContext.get_class(data);
        }
        [Route("get_section")]
        public VikasaSubjectwiseCumulativeReportDTO get_section([FromBody] VikasaSubjectwiseCumulativeReportDTO data)
        {
            return _ReportContext.get_section(data);
        }
        [Route("get_subject")]
        public VikasaSubjectwiseCumulativeReportDTO get_subject([FromBody] VikasaSubjectwiseCumulativeReportDTO data)
        {
            return _ReportContext.get_subject(data);
        }
        [Route("get_Exam")]
        public VikasaSubjectwiseCumulativeReportDTO get_Exam([FromBody] VikasaSubjectwiseCumulativeReportDTO data)
        {
            return _ReportContext.get_Exam(data);
        }

        [Route("savedetails")]
        public async Task<VikasaSubjectwiseCumulativeReportDTO> savedetails([FromBody] VikasaSubjectwiseCumulativeReportDTO data)
        {
            return await _ReportContext.savedetails(data);
        }
    }
}
