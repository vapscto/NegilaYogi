
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
    public class ADMAttendenceFacadeController : Controller
    {
        public ADMAttendenceInterface _ChairmanDashboardReport;

        public ADMAttendenceFacadeController(ADMAttendenceInterface data)
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
        public ADMAttendenceDTO Getdetails([FromBody] ADMAttendenceDTO data)
        {

           
            return  _ChairmanDashboardReport.Getdetails(data);
           
        }

        [HttpPost]
        [Route("getclass")]
        public ADMAttendenceDTO getclass([FromBody] ADMAttendenceDTO data)
        {


            return _ChairmanDashboardReport.getclass(data);

        }
        [HttpPost]
        [Route("Getsection")]
        public ADMAttendenceDTO Getsection([FromBody] ADMAttendenceDTO data)
        {


            return _ChairmanDashboardReport.Getsection(data);

        }
        [HttpPost]
        [Route("GetAttendence")]
        public ADMAttendenceDTO GetAttendence([FromBody] ADMAttendenceDTO data)
        {


            return _ChairmanDashboardReport.GetAttendence(data);

        }

        [HttpPost]
        [Route("GetIndividualAttendence")]
        public ADMAttendenceDTO GetIndividualAttendence([FromBody] ADMAttendenceDTO data)
        {


            return _ChairmanDashboardReport.GetIndividualAttendence(data);

        }



        

    }
}
