using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VisitorsManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VisitorsManagement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VisitorsManagement
{
    [Route("api/[controller]")]
    public class LateInStudentController : Controller
    {
        LateInStudentDelegate deleg = new LateInStudentDelegate();

        [Route("loaddata/{id:int}")]
        public LateInStudent_DTO loaddata(int id)
        {
            LateInStudent_DTO dto = new LateInStudent_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return deleg.loaddata(dto);
        }

        [Route("get_class")]
        public LateInStudent_DTO get_class([FromBody]  LateInStudent_DTO dto)
        {      
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return deleg.get_class(dto);
        }

        [Route("get_section")]
        public LateInStudent_DTO get_section([FromBody]  LateInStudent_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return deleg.get_section(dto);
        }

        [Route("get_student")]
        public LateInStudent_DTO get_student([FromBody]  LateInStudent_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return deleg.get_student(dto);
        }

        [Route("savedata")]
        public LateInStudent_DTO savedata([FromBody]  LateInStudent_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            //dto.ALIEOS_NetworkIP = remoteIpAddress.ToString();

            return deleg.savedata(dto);
        }

        [Route("editdata")]
        public LateInStudent_DTO editdata([FromBody]  LateInStudent_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return deleg.editdata(dto);
        }
        [Route("deactive")]
        public LateInStudent_DTO deactive([FromBody]  LateInStudent_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return deleg.deactive(dto);
        }

    }
}
