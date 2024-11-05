using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.HOD.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.HOD;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.HOD.Controllers
{
    [Route("api/[controller]")]
    public class HODExamSectionPerformanceFacade : Controller
    {

        public HODExamSectionPerformanceInterface _IExamsectionperformance;

        public HODExamSectionPerformanceFacade(HODExamSectionPerformanceInterface parameter)
        {
            _IExamsectionperformance = parameter;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("Getdetails")]
        public HODExamSectionPerformance_DTO Getdetails([FromBody] HODExamSectionPerformance_DTO data)
        {
            return _IExamsectionperformance.Getdetails(data);
        }

        [HttpPost]
        [Route("getcategory")]
        public HODExamSectionPerformance_DTO getclass([FromBody] HODExamSectionPerformance_DTO data)
        {
            return _IExamsectionperformance.getcategory(data);
        }

        [HttpPost]
        [Route("getclassexam")]
        public HODExamSectionPerformance_DTO getclassexam([FromBody] HODExamSectionPerformance_DTO data)
        {
            return _IExamsectionperformance.getclassexam(data);
        }

        [HttpPost]
        [Route("showreport")]
        public HODExamSectionPerformance_DTO showreport([FromBody] HODExamSectionPerformance_DTO data)
        {
            return _IExamsectionperformance.showreport(data);
        }

        [HttpPost]
        [Route("getsubject")]
        public HODExamSectionPerformance_DTO getsubject([FromBody] HODExamSectionPerformance_DTO data)
        {
            return _IExamsectionperformance.getsubject(data);
        }


    }
}
