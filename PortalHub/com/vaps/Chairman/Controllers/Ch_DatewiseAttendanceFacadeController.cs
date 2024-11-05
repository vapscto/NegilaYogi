
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Chairman.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
//using AdmissionServiceHub.com.vaps.Interfaces;


using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Chairman.Controllers
{
    [Route("api/[controller]")]
    public class Ch_DatewiseAttendanceFacadeController : Controller
    {
        public Ch_DatewiseAttendanceInterface _ChairmanDashboardReport;

        public Ch_DatewiseAttendanceFacadeController(Ch_DatewiseAttendanceInterface data)
        {
            _ChairmanDashboardReport = data;
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("Getdetails")]
        public Ch_DatewiseAttendanceDTO Getdetails([FromBody] Ch_DatewiseAttendanceDTO data)
        {

           
            return  _ChairmanDashboardReport.Getdetails(data);
           
        }

        [HttpPost]
        [Route("getclass")]
        public Ch_DatewiseAttendanceDTO getclass([FromBody] Ch_DatewiseAttendanceDTO data)
        {


            return _ChairmanDashboardReport.getclass(data);

        }
        [HttpPost]
        [Route("Getsection")]
        public Ch_DatewiseAttendanceDTO Getsection([FromBody] Ch_DatewiseAttendanceDTO data)
        {


            return _ChairmanDashboardReport.Getsection(data);

        }
        [HttpPost]
        [Route("Getreport")]
        public Ch_DatewiseAttendanceDTO Getreport([FromBody] Ch_DatewiseAttendanceDTO data)
        {


            return _ChairmanDashboardReport.Getreport(data);

        }
        
        



    }
}
