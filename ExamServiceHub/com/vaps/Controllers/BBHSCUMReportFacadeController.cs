
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
//using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class BBHSCUMReportFacadeController : Controller
    {
        public BBHSCUMReportInterface _HHSrptcard;

        public BBHSCUMReportFacadeController(BBHSCUMReportInterface data)
        {
            _HHSrptcard = data;
        }
        

        [HttpPost]
        [Route("Getdetails")]
        public Task<BBHSCUMReportDTO> Getdetails([FromBody] BBHSCUMReportDTO data)
        {


            return _HHSrptcard.Getdetails(data);

        }

        [HttpPost]
        [Route("savedetails")]
        public Task<BBHSCUMReportDTO> savedetails([FromBody] BBHSCUMReportDTO data)
        {


            return _HHSrptcard.savedetails(data);

        }

        [HttpPost]
        [Route("getclass")]
        public BBHSCUMReportDTO getclass([FromBody] BBHSCUMReportDTO data)
        {


            return _HHSrptcard.getclass(data);

        }
        [HttpPost]
        [Route("Getsection")]
        public BBHSCUMReportDTO Getsection([FromBody] BBHSCUMReportDTO data)
        {


            return _HHSrptcard.Getsection(data);

        }
        [HttpPost]
        [Route("GetAttendence")]
        public BBHSCUMReportDTO GetAttendence([FromBody] BBHSCUMReportDTO data)
        {


            return _HHSrptcard.GetAttendence(data);

        }

        //[HttpPost]
        //[Route("GetIndividualAttendence")]
        //public BBHSCUMReportDTO GetIndividualAttendence([FromBody] BBHSCUMReportDTO data)
        //{


        //    return _HHSrptcard.GetIndividualAttendence(data);

        //}




    }
}
