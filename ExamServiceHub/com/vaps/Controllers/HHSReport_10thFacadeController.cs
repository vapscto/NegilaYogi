
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
    public class HHSReport_10thFacadeController : Controller
    {
        public HHSReport_10thInterface _HHSrptcard;

        public HHSReport_10thFacadeController(HHSReport_10thInterface data)
        {
            _HHSrptcard = data;
        }
        

        [HttpPost]
        [Route("Getdetails")]
        public Task<HHSReport_10thDTO> Getdetails([FromBody] HHSReport_10thDTO data)
        {


            return _HHSrptcard.Getdetails(data);

        }

        [HttpPost]
        [Route("savedetails")]
        public Task<HHSReport_10thDTO> savedetails([FromBody] HHSReport_10thDTO data)
        {


            return _HHSrptcard.savedetails(data);

        }

        [HttpPost]
        [Route("getclass")]
        public HHSReport_10thDTO getclass([FromBody] HHSReport_10thDTO data)
        {


            return _HHSrptcard.getclass(data);

        }
        [HttpPost]
        [Route("Getsection")]
        public HHSReport_10thDTO Getsection([FromBody] HHSReport_10thDTO data)
        {


            return _HHSrptcard.Getsection(data);

        }
        [HttpPost]
        [Route("GetAttendence")]
        public HHSReport_10thDTO GetAttendence([FromBody] HHSReport_10thDTO data)
        {


            return _HHSrptcard.GetAttendence(data);

        }

        //[HttpPost]
        //[Route("GetIndividualAttendence")]
        //public HHSReport_10thDTO GetIndividualAttendence([FromBody] HHSReport_10thDTO data)
        //{


        //    return _HHSrptcard.GetIndividualAttendence(data);

        //}




    }
}
