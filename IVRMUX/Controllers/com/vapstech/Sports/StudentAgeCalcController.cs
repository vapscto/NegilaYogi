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
    public class StudentAgeCalcController : Controller
    {
        StudentAgeCalcDelegate delegat = new StudentAgeCalcDelegate();

        [Route("loadgrid/{id:int}")]
        public StudentAgeCalcDTO Getdetails(int id)
        {
            StudentAgeCalcDTO dto = new StudentAgeCalcDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            
            return delegat.Getdetails(dto);
        }

        [Route("getStudents")]
        public StudentAgeCalcDTO getStudents([FromBody]StudentAgeCalcDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           // data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delegat.getStudents(data);
        }

        [Route("saveRecord")]
        public StudentAgeCalcDTO saveRecord([FromBody]StudentAgeCalcDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.save(data);
        }

        [Route("report")]
        public StudentAgeCalcDTO report([FromBody]StudentAgeCalcDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.report(data);
        }

        [Route("Get_Class_House")]
        public StudentAgeCalcDTO Get_Class_House(StudentAgeCalcDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.Get_Class_House(data);
        }
    }
}
