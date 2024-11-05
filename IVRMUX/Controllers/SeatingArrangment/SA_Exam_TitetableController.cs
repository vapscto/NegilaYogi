using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.SeatingArrangment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.SeatingArrangment;

namespace IVRMUX.Controllers.SeatingArrangment
{

    [Route("api/[controller]")]
    public class SA_Exam_TitetableController : Controller
    {
        SA_Exam_TitetableDelegate ETD = new SA_Exam_TitetableDelegate();

        [Route("load_TT/{id:int}")]
        public SA_Exam_TitetableDTO load_TT(int id)
        {
            SA_Exam_TitetableDTO dto = new SA_Exam_TitetableDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return ETD.load_TT(dto);
        }

        [Route("Save_TT")]
        public SA_Exam_TitetableDTO Save_TT([FromBody] SA_Exam_TitetableDTO dto)
        {
            
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return ETD.Save_TT(dto);
        }

        [Route("Edit_TT")]
        public SA_Exam_TitetableDTO Edit_TT([FromBody] SA_Exam_TitetableDTO dto)
        {
            
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return ETD.Edit_TT(dto);
        }

        [Route("Deactive_TT")]
        public SA_Exam_TitetableDTO Deactive_TT([FromBody] SA_Exam_TitetableDTO dto)
        {
            
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return ETD.Deactive_TT(dto);
        }

        [Route("viewTTdetails")]
        public SA_Exam_TitetableDTO viewTTdetails([FromBody] SA_Exam_TitetableDTO dto)
        {
            
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return ETD.viewTTdetails(dto);
        }
    }
}