using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using corewebapi18072016.Delegates.com.vapstech.College.Portals.Student;
using PreadmissionDTOs.com.vaps.College.Student;

namespace corewebapi18072016.Controllers.com.vapstech.College
{

    [Route("api/[controller]")]
    public class CollegeStudent_TTController : Controller
    {
        CollegeStudent_TTDelegate fdd = new CollegeStudent_TTDelegate();

        [Route("getloaddata/{id:int}")]
        public CollegeStudent_TTDTO getloaddata(int id)
        {
            CollegeStudent_TTDTO data = new CollegeStudent_TTDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMCST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return fdd.getloaddata(data);
        }


        [Route("getStudentTT")]
        public CollegeStudent_TTDTO getStudentTT([FromBody]CollegeStudent_TTDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMCST_Id"));
           
            return fdd.getStudentTT(sddto);
        }


    }
}
