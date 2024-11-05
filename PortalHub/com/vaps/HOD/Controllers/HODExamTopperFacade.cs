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
    public class HODExamTopperFacade : Controller
    {
        public HODExamTopperInterface _HODExamTopperReport;

        public HODExamTopperFacade(HODExamTopperInterface data)
        {
            _HODExamTopperReport = data;
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("Getdetails")]
        public HODExamTopper_DTO Getdetails([FromBody] HODExamTopper_DTO data)
        {
            return _HODExamTopperReport.Getdetails(data);
        }

        [HttpPost]
        [Route("getcategory")]
        public HODExamTopper_DTO getclass([FromBody] HODExamTopper_DTO data)
        {
            return _HODExamTopperReport.getcategory(data);

        }

        [HttpPost]
        [Route("getclassexam")]
        public HODExamTopper_DTO getclassexam([FromBody] HODExamTopper_DTO data)
        {
            return _HODExamTopperReport.getclassexam(data);
        }

        [HttpPost]
        [Route("showreport")]
        public HODExamTopper_DTO showreport([FromBody] HODExamTopper_DTO data)
        {
            return _HODExamTopperReport.showreport(data);
        }

        [HttpPost]
        [Route("getsection")]
        public HODExamTopper_DTO getsection([FromBody] HODExamTopper_DTO data)
        {
            return _HODExamTopperReport.getsection(data);
        }

    }
}
