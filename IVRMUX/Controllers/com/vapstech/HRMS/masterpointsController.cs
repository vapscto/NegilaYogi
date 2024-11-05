using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using corewebapi18072016.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
namespace corewebapi18072016.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class masterpointsController : Controller
    {
        masterpointsDelegate del = new masterpointsDelegate();

        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public HR_Employee_AssesmentpointsDTO getalldetails(int id)
        {
            HR_Employee_AssesmentpointsDTO dto = new HR_Employee_AssesmentpointsDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.onloadgetdetails(dto);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public HR_Employee_AssesmentpointsDTO Post([FromBody]HR_Employee_AssesmentpointsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.savedetails(dto);
        }

        [Route("editRecord/{id:int}")]
        public HR_Employee_AssesmentpointsDTO editRecord(int id)
        {
            HR_Employee_AssesmentpointsDTO dto = new HR_Employee_AssesmentpointsDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getRecorddetailsById(id);
        }

        [Route("ActiveDeactiveRecord/{id:int}")]
        public HR_Employee_AssesmentpointsDTO ActiveDeactiveRecord(int id)
        {
            HR_Employee_AssesmentpointsDTO dto = new HR_Employee_AssesmentpointsDTO();
            dto.HR_Emp_As_Opid = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.deleterec(dto);
        }
    }
}
