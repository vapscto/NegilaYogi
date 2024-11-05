using System;
using IVRMUX.Delegates.com.vapstech.College.Fees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fee;

namespace IVRMUX.Controllers.com.vapstech.College.Fees
{
    [Route("api/[controller]")]
    public class CLGStudentFeeEnablePartialPaymentController : Controller
    {
        public CLGStudentFeeEnablePartialPaymentDelegate objDel = new CLGStudentFeeEnablePartialPaymentDelegate();

        [HttpGet]
        [Route("GetYearList/{id:int}")]
        public CollegeOverallFeeStatusDTO GetYearList(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.GetYearList(id);
        }
        [HttpPost]
        [Route("get_courses")]
        public CollegeOverallFeeStatusDTO get_courses([FromBody]CollegeOverallFeeStatusDTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            id.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objDel.get_courses(id);
        }

        //[HttpPost]
        [Route("get_branches")]
        public CollegeOverallFeeStatusDTO get_branches([FromBody]CollegeOverallFeeStatusDTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            id.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objDel.get_branches(id);
        }
        //[HttpPost]
        [Route("get_semisters")]
        public CollegeOverallFeeStatusDTO get_semisters([FromBody]CollegeOverallFeeStatusDTO Data)
        {
            Data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.get_semisters(Data);
        }
        [Route("get_student")]
        public CollegeOverallFeeStatusDTO get_student([FromBody]CollegeOverallFeeStatusDTO Data)
        {
            Data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.get_student(Data);
        }
        [Route("savedata")]
        public CollegeOverallFeeStatusDTO savedata([FromBody] CollegeOverallFeeStatusDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));          
            return objDel.savedata(data);
        }
        [Route("deactivate")]
        public CollegeOverallFeeStatusDTO deactivate([FromBody] CollegeOverallFeeStatusDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));          
            return objDel.deactivate(data);
        }
    }
}