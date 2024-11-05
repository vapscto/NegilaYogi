
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
    public class HHSReport_5to7FacadeController : Controller
    {
        public HHSReport_5to7Interface _HHSrptcard;
        public HHSReport_5to7FacadeController(HHSReport_5to7Interface data)
        {
            _HHSrptcard = data;
        }        

        [HttpPost]
        [Route("Getdetails")]
        public Task<HHSReport_5to7DTO> Getdetails([FromBody] HHSReport_5to7DTO data)
        {
            return _HHSrptcard.Getdetails(data);
        }

        [HttpPost]
        [Route("savedetails")]
        public Task<HHSReport_5to7DTO> savedetails([FromBody] HHSReport_5to7DTO data)
        {
            return _HHSrptcard.savedetails(data);
        }

        [HttpPost]
        [Route("getclass")]
        public HHSReport_5to7DTO getclass([FromBody] HHSReport_5to7DTO data)
        {
            return _HHSrptcard.getclass(data);
        }

        [HttpPost]
        [Route("Getsection")]
        public HHSReport_5to7DTO Getsection([FromBody] HHSReport_5to7DTO data)
        {
            return _HHSrptcard.Getsection(data);
        }

        [HttpPost]
        [Route("GetAttendence")]
        public HHSReport_5to7DTO GetAttendence([FromBody] HHSReport_5to7DTO data)
        {
            return _HHSrptcard.GetAttendence(data);
        }
         
        [Route("Get_primary_savedetails")]
        public HHSReport_5to7DTO Get_primary_savedetails([FromBody] HHSReport_5to7DTO data)
        {
            return _HHSrptcard.Get_primary_savedetails(data);
        }
    }
}