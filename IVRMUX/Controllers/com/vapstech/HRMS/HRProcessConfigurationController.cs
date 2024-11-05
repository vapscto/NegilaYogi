using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.HRMS;
using Microsoft.AspNetCore.Http;

namespace corewebapi18072016.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class HRProcessConfigurationController : Controller
    {
        HRProcessConfigurationDelegate hrdel = new HRProcessConfigurationDelegate();

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public HR_ProcessDTO getalldetails(int id)
        {
            HR_ProcessDTO dto = new HR_ProcessDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return hrdel.onloadgetdetails(dto);
        }
        // POST api/values
        [HttpPost]
        [Route("savedata")]
        public HR_ProcessDTO savedata([FromBody] HR_ProcessDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            //dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return hrdel.savedetails(dto);
        }

        [Route("editRecord/{id:int}")]
        public HR_ProcessDTO editRecord(int id)
        {
            HR_ProcessDTO dto = new HR_ProcessDTO();
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return hrdel.getRecorddetailsById(id);
        }

        [Route("ActiveDeactiveRecord/{id:int}")]
        public HR_ProcessDTO ActiveDeactiveRecord(int id)
        {
            HR_ProcessDTO dto = new HR_ProcessDTO();

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return hrdel.deleterec(id);
        }

        [HttpPost]
        [Route("deleteauth")]
        public HR_ProcessDTO deleteauth([FromBody] HR_ProcessDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return hrdel.deleteauth(data);
        }


    }
}
