using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontOffice.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FrontOffice.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class StudentInOutReportFacade : Controller
    {
        public StudentInOutReportInterface _interface;

        public StudentInOutReportFacade(StudentInOutReportInterface _inter)
        {
            _interface = _inter;
        }
        
        [Route("loaddata")]
        public StudentInOutReportDTO loaddata([FromBody] StudentInOutReportDTO data)
        {
            return _interface.loaddata(data);
        }
        //getsection
        [Route("getsection")]
        public StudentInOutReportDTO getsection([FromBody] StudentInOutReportDTO data)
        {
            return _interface.getsection(data);
        }
        //getstudent
        [Route("getstudent")]
        public StudentInOutReportDTO getstudent([FromBody] StudentInOutReportDTO data)
        {
            return _interface.getstudent(data);
        }
        //report
        [Route("report")]
        public StudentInOutReportDTO report([FromBody] StudentInOutReportDTO data)
        {
            return _interface.report(data);
        }
    }
}
