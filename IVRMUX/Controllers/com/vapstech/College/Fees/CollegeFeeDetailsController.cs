using System;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fee;
using IVRMUX.Delegates.com.vapstech.College.Fees;
using Microsoft.AspNetCore.Http;

namespace IVRMUX.Controllers.com.vapstech.College.Fees
{
    [Route("api/[controller]")]
        public class CollegeFeeDetailsController : Controller
        {

            // GET: api/values
            public CollegeFeeDetailsDelegate objDel = new CollegeFeeDetailsDelegate();

            [HttpGet]
            [Route("GetYearList/{id:int}")]
            public CollegeStudentLedgerDTO GetYearList(int id)
            {
                id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                return objDel.GetYearList(id);
            }
            [HttpPost]
            [Route("get_courses")]
            public CollegeStudentLedgerDTO get_courses([FromBody]CollegeStudentLedgerDTO id)
            {
                id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                id.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                return objDel.get_courses(id);
            }

            //[HttpPost]
            [Route("get_branches")]
            public CollegeStudentLedgerDTO get_branches([FromBody]CollegeStudentLedgerDTO id)
            {
                id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

                id.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                return objDel.get_branches(id);
            }
            //[HttpPost]
            [Route("get_semisters")]
            public CollegeStudentLedgerDTO get_semisters([FromBody]CollegeStudentLedgerDTO Data)
            {
                Data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                return objDel.get_semisters(Data);
            }

          
            //[HttpPost]
            [Route("get_report")]
            public CollegeStudentLedgerDTO get_report([FromBody]CollegeStudentLedgerDTO Data)
            {
                Data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                Data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                return objDel.get_report(Data);
            }

        [Route("get_concession_report")]
        public CollegeStudentLedgerDTO get_concession_report([FromBody]CollegeStudentLedgerDTO Data)
        {
            Data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            Data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objDel.get_concession_report(Data);
        }
        


    }
    
   
}
