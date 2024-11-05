using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Sports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class SportsStudentHouseMappingController : Controller
    {
        // GET: api/<controller>
        SportsStudentHouseMappingDelegate delegat = new SportsStudentHouseMappingDelegate();

        [Route("getdetails/{id:int}")]
        public SPCC_Student_House_DTO getdetails(int id)
        {
            SPCC_Student_House_DTO dto = new SPCC_Student_House_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delegat.getdetails(dto);
        }

        [Route("get_class")]
        public SPCC_Student_House_DTO get_class([FromBody] SPCC_Student_House_DTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return delegat.get_class(d);
        }

        [Route("get_section")]
        public SPCC_Student_House_DTO get_section([FromBody] SPCC_Student_House_DTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return delegat.get_section(d);
        }

        [Route("get_student")]
        public SPCC_Student_House_DTO get_student([FromBody] SPCC_Student_House_DTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return delegat.get_student(d);
        }

        [Route("get_student_info")]
        public SPCC_Student_House_DTO get_student_info([FromBody] SPCC_Student_House_DTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.get_student_info(d);
        }

        [Route("saveRecord")]
        public SPCC_Student_House_DTO saveRecord([FromBody]SPCC_Student_House_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.saveRecord(data);
        }

        [Route("EditRecord")]
        public SPCC_Student_House_DTO EditRecord([FromBody]SPCC_Student_House_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            
            return delegat.EditRecord(data);
        }

        [Route("deactivate")]
        public SPCC_Student_House_DTO deactivate([FromBody] SPCC_Student_House_DTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.deactivate(d);
        }

     
        
    }
}
