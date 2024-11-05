
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
    public class HHSReport_PreNursFacadeController : Controller
    {
        public HHSReport_PreNursInterface _HHSrptcard;

        public HHSReport_PreNursFacadeController(HHSReport_PreNursInterface data)
        {
            _HHSrptcard = data;
        }
        

        [HttpPost]
        [Route("Getdetails")]
        public Task<HHSReport_PreNursDTO> Getdetails([FromBody] HHSReport_PreNursDTO data)
        {


            return _HHSrptcard.Getdetails(data);

        }

        [HttpPost]
        [Route("savedetails")]
        public Task<HHSReport_PreNursDTO> savedetails([FromBody] HHSReport_PreNursDTO data)
        {


            return _HHSrptcard.savedetails(data);

        }

        [HttpPost]
        [Route("getclass")]
        public HHSReport_PreNursDTO getclass([FromBody] HHSReport_PreNursDTO data)
        {


            return _HHSrptcard.getclass(data);

        }
        [HttpPost]
        [Route("Getsection")]
        public HHSReport_PreNursDTO Getsection([FromBody] HHSReport_PreNursDTO data)
        {


            return _HHSrptcard.Getsection(data);

        }
        [HttpPost]
        [Route("GetAttendence")]
        public HHSReport_PreNursDTO GetAttendence([FromBody] HHSReport_PreNursDTO data)
        {


            return _HHSrptcard.GetAttendence(data);

        }

        //[HttpPost]
        //[Route("GetIndividualAttendence")]
        //public HHSReport_PreNursDTO GetIndividualAttendence([FromBody] HHSReport_PreNursDTO data)
        //{


        //    return _HHSrptcard.GetIndividualAttendence(data);

        //}




    }
}
