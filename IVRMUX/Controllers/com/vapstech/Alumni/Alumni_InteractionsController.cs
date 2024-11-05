using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Alumni;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Alumni;

namespace IVRMUX.Controllers.com.vapstech.Alumni
{
    
    [Route("api/[controller]")]
    public class Alumni_InteractionsController : Controller
    {
        public Alumni_Interactions_Delegate _del = new Alumni_Interactions_Delegate();

        [Route("getloaddata/{id:int}")]
        public Alumni_School_Interactions_DTO getloaddata(int id)
        {
            Alumni_School_Interactions_DTO data = new Alumni_School_Interactions_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ALMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ALMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ALMST_Id"));
            return _del.getloaddata(data);
        }
        [Route("getdetails")]
        public Alumni_School_Interactions_DTO getdetails([FromBody] Alumni_School_Interactions_DTO data)
        {
            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ALMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ALMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ALMST_Id"));
            return _del.getdetails(data);
        }
        [Route("savedetails")]
        public Alumni_School_Interactions_DTO savedetails([FromBody] Alumni_School_Interactions_DTO data)
        {
            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ALMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ALMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ALMST_Id"));
            return _del.savedetails(data);
        }

         [Route("reply")]
        public Alumni_School_Interactions_DTO reply([FromBody] Alumni_School_Interactions_DTO data)
        {
            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ALMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ALMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ALMST_Id"));
            return _del.reply(data);
        }
         [Route("savereply")]
        public Alumni_School_Interactions_DTO savereply([FromBody] Alumni_School_Interactions_DTO data)
        {
            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ALMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ALMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ALMST_Id"));
            return _del.savereply(data);
        }
        

    }
}