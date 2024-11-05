using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
using ExamServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class VikasaProgressReportExamFacadeController : Controller
    {
        public VikasaProgressReportExamInterface _PCReportContext;

        public VikasaProgressReportExamFacadeController(VikasaProgressReportExamInterface dt)
        {
            _PCReportContext = dt;
        }


        [Route("Getdetails")]
        public VikasaSubjectwiseCumulativeReportDTO Getdetails([FromBody]VikasaSubjectwiseCumulativeReportDTO obj)//int IVRMM_Id
        {
            return _PCReportContext.Getdetails(obj);
        }


        [Route("savedetails")]
        public Task<VikasaSubjectwiseCumulativeReportDTO> showdetails([FromBody] VikasaSubjectwiseCumulativeReportDTO data)
        {
            return _PCReportContext.showdetails(data);
        }

        [Route("get_class")]
        public VikasaSubjectwiseCumulativeReportDTO get_class([FromBody] VikasaSubjectwiseCumulativeReportDTO data)
        {
            return _PCReportContext.get_class(data);
        }
        [Route("get_section")]
        public VikasaSubjectwiseCumulativeReportDTO get_section([FromBody] VikasaSubjectwiseCumulativeReportDTO data)
        {
            return _PCReportContext.get_section(data);
        }

        [Route("get_exam")]
        public VikasaSubjectwiseCumulativeReportDTO get_exam([FromBody] VikasaSubjectwiseCumulativeReportDTO data)
        {
            return _PCReportContext.get_exam(data);
        }
        [Route("savedetailsnew")]
        public Task<VikasaSubjectwiseCumulativeReportDTO> savedetailsnew([FromBody] VikasaSubjectwiseCumulativeReportDTO data)
        {
            return _PCReportContext.savedetailsnew(data);
        }
        [Route("cbsesavedetails")]
        public Task<VikasaSubjectwiseCumulativeReportDTO> cbsesavedetails([FromBody] VikasaSubjectwiseCumulativeReportDTO data)
        {
            return _PCReportContext.cbsesavedetails(data);
        }
        [Route("aggregativereport")]
        public Task<VikasaSubjectwiseCumulativeReportDTO> aggregativereport([FromBody] VikasaSubjectwiseCumulativeReportDTO data)
        {
            return _PCReportContext.aggregativereport(data);
        }
        
    }
}
