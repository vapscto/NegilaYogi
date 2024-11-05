using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.HealthManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.HealthManagement;

namespace IVRMUX.Controllers.HealthManagement
{

    [Route("api/[controller]")]

    public class HM_MasterController : Controller
    {
        HM_MasterDelegate _del = new HM_MasterDelegate();

        // Master Behaviour
        [Route("load_MB/{id:int}")]
        public Master_HealthManagementDTO load_MB(int id)
        {
            Master_HealthManagementDTO dto = new Master_HealthManagementDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _del.load_MB(dto);
        }

        [Route("Save_MB")]
        public Master_HealthManagementDTO save_MB([FromBody] Master_HealthManagementDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _del.Save_MB(dto);
        }
        [Route("Edit_MB")]
        public Master_HealthManagementDTO Edit_MB([FromBody] Master_HealthManagementDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _del.Edit_MB(dto);
        }
        [Route("ActiveDeactive_MB")]
        public Master_HealthManagementDTO ActiveDeactive_MB([FromBody] Master_HealthManagementDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _del.ActiveDeactive_MB(dto);
        }

        // Master Cleanness
        [Route("load_CL/{id:int}")]
        public Master_HealthManagementDTO load_CL(int id)
        {
            Master_HealthManagementDTO dto = new Master_HealthManagementDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _del.load_CL(dto);
        }

        [Route("Save_CL")]
        public Master_HealthManagementDTO save_CL([FromBody] Master_HealthManagementDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _del.Save_CL(dto);
        }
        [Route("Edit_CL")]
        public Master_HealthManagementDTO Edit_CL([FromBody] Master_HealthManagementDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _del.Edit_CL(dto);
        }
        [Route("ActiveDeactive_CL")]
        public Master_HealthManagementDTO ActiveDeactive_CL([FromBody] Master_HealthManagementDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _del.ActiveDeactive_CL(dto);
        }

        // Master Doctor
        [Route("load_DC/{id:int}")]
        public Master_HealthManagementDTO load_DC(int id)
        {
            Master_HealthManagementDTO dto = new Master_HealthManagementDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _del.load_DC(dto);
        }

        [Route("Save_DC")]
        public Master_HealthManagementDTO save_DC([FromBody] Master_HealthManagementDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _del.Save_DC(dto);
        }
        [Route("Edit_DC")]
        public Master_HealthManagementDTO Edit_DC([FromBody] Master_HealthManagementDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _del.Edit_DC(dto);
        }
        [Route("ActiveDeactive_DC")]
        public Master_HealthManagementDTO ActiveDeactive_DC([FromBody] Master_HealthManagementDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _del.ActiveDeactive_DC(dto);
        }

        // Master Examination
        [Route("load_EX/{id:int}")]
        public Master_HealthManagementDTO load_EX(int id)
        {
            Master_HealthManagementDTO dto = new Master_HealthManagementDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _del.load_EX(dto);
        }

        [Route("Save_EX")]
        public Master_HealthManagementDTO save_EX([FromBody] Master_HealthManagementDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _del.Save_EX(dto);
        }
        [Route("Edit_EX")]
        public Master_HealthManagementDTO Edit_EX([FromBody] Master_HealthManagementDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _del.Edit_EX(dto);
        }
        [Route("ActiveDeactive_EX")]
        public Master_HealthManagementDTO ActiveDeactive_EX([FromBody] Master_HealthManagementDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _del.ActiveDeactive_EX(dto);
        }

        // Master Observation
        [Route("load_OB/{id:int}")]
        public Master_HealthManagementDTO load_OB(int id)
        {
            Master_HealthManagementDTO dto = new Master_HealthManagementDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _del.load_OB(dto);
        }

        [Route("Save_OB")]
        public Master_HealthManagementDTO save_OB([FromBody] Master_HealthManagementDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _del.Save_OB(dto);
        }
        [Route("Edit_OB")]
        public Master_HealthManagementDTO Edit_OB([FromBody] Master_HealthManagementDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _del.Edit_OB(dto);
        }

        [Route("ActiveDeactive_OB")]
        public Master_HealthManagementDTO ActiveDeactive_OB([FromBody] Master_HealthManagementDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _del.ActiveDeactive_OB(dto);
        }

        // Master Illness

        [Route("Load_illness/{id:int}")]
        public Master_HealthManagementDTO Load_illness(int id)
        {
            Master_HealthManagementDTO dto = new Master_HealthManagementDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _del.Load_illness(dto);
        }

        [Route("Save_illness")]
        public Master_HealthManagementDTO Save_illness([FromBody] Master_HealthManagementDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _del.Save_illness(dto);
        } 

        [Route("Edit_illness")]
        public Master_HealthManagementDTO Edit_illness([FromBody] Master_HealthManagementDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _del.Edit_illness(dto);
        }  

        [Route("ActiveDeactive_illness")]
        public Master_HealthManagementDTO ActiveDeactive_illness([FromBody] Master_HealthManagementDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _del.ActiveDeactive_illness(dto);
        }        
    }
}