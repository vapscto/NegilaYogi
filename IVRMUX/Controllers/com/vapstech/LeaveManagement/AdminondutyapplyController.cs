using System;
using corewebapi18072016.Delegates.com.vapstech.LeaveManagement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.LeaveManagement;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.LeaveManagement
{
    [Route("api/[controller]")]
    public class AdminondutyapplyController : Controller
    {
        AdminondutyapplyDelegate returnval = new AdminondutyapplyDelegate();
        [Route("Getdetails/{id:int}")]
        public AdminondutyapplyDTO Getdetails(int id)
        {
            AdminondutyapplyDTO data = new AdminondutyapplyDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return returnval.GetData(data);
        }
        [Route("employeedetails")]
        public AdminondutyapplyDTO employeedetails([FromBody] AdminondutyapplyDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return returnval.employeedetails(data);
        }

        [Route("requestleave")]
        public AdminondutyapplyDTO requestleave([FromBody] AdminondutyapplyDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return returnval.requestleave(data);
        }
        [Route("viewcomment")]
        public AdminondutyapplyDTO viewcomment([FromBody] AdminondutyapplyDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return returnval.viewcomment(data);
        }
        [Route("ActiveDeactiveRecord/{id:int}")]
        public AdminondutyapplyDTO ActiveDeactiveRecord(int id)
        {
            AdminondutyapplyDTO dto = new AdminondutyapplyDTO();
            dto.HRELAP_Id = id;           
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return returnval.ActiveDeactiveRecord(dto);
        }

        [Route("editData")]
        public AdminondutyapplyDTO editData([FromBody] AdminondutyapplyDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return returnval.editData(data);
        }
    }
    }

