using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.SeatingArrangment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.SeatingArrangment;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.SeatingArrangment
{
    [Route("api/[controller]")]
    public class SAMasterSuperintendentController : Controller
    {
        SAMasterSuperintendentDelegate _delg = new SAMasterSuperintendentDelegate();
        // ===============superintendent==================
        [Route("load_sup/{id:int}")]
        public SAMasterSuperintendent load_sup(int id)
        {
            SAMasterSuperintendent dto = new SAMasterSuperintendent();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId")); 
            return _delg.load_sup(dto);
        }

        [Route("Save_sup")]
        public SAMasterSuperintendent Save_sup([FromBody] SAMasterSuperintendent dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId")); 
            return _delg.Save_sup(dto);
        }

        [Route("Edit_sup")]
        public SAMasterSuperintendent Edit_sup([FromBody] SAMasterSuperintendent dto)
        {
            SAMasterSuperintendent data = new SAMasterSuperintendent();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId")); 
            return _delg.Edit_sup(dto);
        }

        [Route("ActiveDeactive_sup")]
        public SAMasterSuperintendent ActiveDeactive_sup([FromBody] SAMasterSuperintendent dto)
        {
            SAMasterSuperintendent data = new SAMasterSuperintendent();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId")); 
            return _delg.ActiveDeactive_sup(dto);
        }

        //=========Absent Student======================

        [Route("load_AS/{id:int}")]
        public SAMasterSuperintendent load_AS(int id)
        {
            SAMasterSuperintendent dto = new SAMasterSuperintendent();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.load_AS(dto);
        }

        [Route("Save_AS")]
        public SAMasterSuperintendent Save_AS([FromBody] SAMasterSuperintendent dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.Save_AS(dto);
        }

        [Route("Edit_AS")]
        public SAMasterSuperintendent Edit_AS([FromBody] SAMasterSuperintendent dto)
        {
            SAMasterSuperintendent data = new SAMasterSuperintendent();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.Edit_AS(dto);
        }

        [Route("DeleteAbsentStudent")]
        public SAMasterSuperintendent DeleteAbsentStudent([FromBody] SAMasterSuperintendent dto)
        {
            SAMasterSuperintendent data = new SAMasterSuperintendent();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.DeleteAbsentStudent(dto);
        }


        //=========Malpractice Student======================

        [Route("load_MPS/{id:int}")]
        public SAMasterSuperintendent load_MPS(int id)
        {
            SAMasterSuperintendent dto = new SAMasterSuperintendent();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.load_MPS(dto);
        }

        [Route("Save_MPS")]
        public SAMasterSuperintendent Save_MPS([FromBody] SAMasterSuperintendent dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.Save_MPS(dto);
        }

        [Route("Edit_MPS")]
        public SAMasterSuperintendent Edit_MPS([FromBody] SAMasterSuperintendent dto)
        {
            SAMasterSuperintendent data = new SAMasterSuperintendent();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.Edit_MPS(dto);
        }

        [Route("DeleteMalPraticeStudent")]
        public SAMasterSuperintendent DeleteMalPraticeStudent([FromBody] SAMasterSuperintendent dto)
        {
            SAMasterSuperintendent data = new SAMasterSuperintendent();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.DeleteMalPraticeStudent(dto);
        }
        

        //=========Chief coordinator======================

        [Route("load_CC/{id:int}")]
        public SAMasterSuperintendent load_CC(int id)
        {
            SAMasterSuperintendent dto = new SAMasterSuperintendent();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.load_CC(dto);
        }

        [Route("Save_CC")]
        public SAMasterSuperintendent Save_CC([FromBody] SAMasterSuperintendent dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.Save_CC(dto);
        }
        [Route("Edit_CC")]
        public SAMasterSuperintendent Edit_CC([FromBody] SAMasterSuperintendent dto)
        {
            SAMasterSuperintendent data = new SAMasterSuperintendent();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.Edit_CC(dto);
        }

        [Route("ActiveDeactive_CC")]
        public SAMasterSuperintendent ActiveDeactive_CC([FromBody] SAMasterSuperintendent dto)
        {
            SAMasterSuperintendent data = new SAMasterSuperintendent();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.ActiveDeactive_CC(dto);
        }

        //***************** General Selection  Details *********************//
        [Route("GetCourse")]
        public SAMasterSuperintendent GetCourse([FromBody] SAMasterSuperintendent dto)
        {
            SAMasterSuperintendent data = new SAMasterSuperintendent();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.GetCourse(dto);
        }

        [Route("GetBranch")]
        public SAMasterSuperintendent GetBranch([FromBody] SAMasterSuperintendent dto)
        {
            SAMasterSuperintendent data = new SAMasterSuperintendent();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.GetBranch(dto);
        }

        [Route("GetSemester")]
        public SAMasterSuperintendent GetSemester([FromBody] SAMasterSuperintendent dto)
        {
            SAMasterSuperintendent data = new SAMasterSuperintendent();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.GetSemester(dto);
        }

        [Route("GetSubject")]
        public SAMasterSuperintendent GetSubject([FromBody] SAMasterSuperintendent dto)
        {
            SAMasterSuperintendent data = new SAMasterSuperintendent();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.GetSubject(dto);
        }
        [Route("GetStudent")]
        public SAMasterSuperintendent GetStudent([FromBody] SAMasterSuperintendent dto)
        {
            SAMasterSuperintendent data = new SAMasterSuperintendent();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.GetStudent(dto);
        }
    }
}