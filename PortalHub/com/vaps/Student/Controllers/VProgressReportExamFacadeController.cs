using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
using PortalHub.com.vaps.Interfaces;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class VProgressReportExamFacadeController : Controller
    {
        public VProgressReportExamInterface _PCReportContext;

        public VProgressReportExamFacadeController(VProgressReportExamInterface dt)
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


        [Route("get_exam")]
        public VikasaSubjectwiseCumulativeReportDTO get_exam([FromBody] VikasaSubjectwiseCumulativeReportDTO data)
        {
            return _PCReportContext.get_exam(data);
        }
        [Route("get_Category")]
        public VikasaSubjectwiseCumulativeReportDTO get_Category([FromBody] VikasaSubjectwiseCumulativeReportDTO data)
        {
            return _PCReportContext.get_Category(data);
        }
        [Route("aggregativereport")]
        public Task<VikasaSubjectwiseCumulativeReportDTO> aggregativereport([FromBody] VikasaSubjectwiseCumulativeReportDTO data)
        {
            return _PCReportContext.aggregativereport(data);
        }
    }
}
