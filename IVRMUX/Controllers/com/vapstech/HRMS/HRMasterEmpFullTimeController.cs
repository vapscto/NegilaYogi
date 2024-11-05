using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class HRMasterEmpFullTimeController : Controller
    {
        HRMasterEmpFullTimeDelegate del = new HRMasterEmpFullTimeDelegate();

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("getalldetails/{id:int}")]
        public NAACHRMasterEmpFullTimeDTO getalldetails(int id)
        {
            NAACHRMasterEmpFullTimeDTO dto = new NAACHRMasterEmpFullTimeDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getalldetails(dto);
        }

        [Route("savedata")]
        public NAACHRMasterEmpFullTimeDTO savedata ([FromBody]NAACHRMasterEmpFullTimeDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.savedata(dto);
        }

        [Route("editRecord/{id:int}")]
        public NAACHRMasterEmpFullTimeDTO editRecord(int id)
        {
            NAACHRMasterEmpFullTimeDTO dto = new NAACHRMasterEmpFullTimeDTO();
            dto.HRMEPT_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.editRecord(dto);
        }

        [Route("ActiveDeactiveRecord/{id:int}")]
        public NAACHRMasterEmpFullTimeDTO ActiveDeactiveRecord(int id)
        {
            NAACHRMasterEmpFullTimeDTO dto = new NAACHRMasterEmpFullTimeDTO();
            dto.HRMEPT_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.ActiveDeactiveRecord(dto);
        }
    }
}
