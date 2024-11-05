using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fee;
using corewebapi18072016.Delegates.com.vapstech.College.Fees.Masters;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Fees.Masters
{
    [Route("api/[controller]")]
    public class CollegeStudentLedgerController : Controller
    {
       
        // GET: api/values
        public CollegeStudentLedgerDelegate objDel = new CollegeStudentLedgerDelegate();

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

        [Route("get_student")]
        public CollegeStudentLedgerDTO get_student([FromBody]CollegeStudentLedgerDTO Data)
        {
            Data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.get_student(Data);
        }
        //[HttpPost]
        [Route("get_report")]
        public CollegeStudentLedgerDTO get_report([FromBody]CollegeStudentLedgerDTO Data)
        {
            Data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            Data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objDel.get_report(Data);
        }

        [Route("savedata")]
        public CollegeStudentLedgerDTO savedata([FromBody] CollegeStudentLedgerDTO pgmodu)
        {
            
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //pgmodu.MI_Id = 2;
            pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objDel.savedata(pgmodu);
        }

      [Route("editdata")]
      public CollegeStudentLedgerDTO editdata([FromBody]CollegeStudentLedgerDTO data)
        {
            data.MI_Id =Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id= Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return objDel.editdata(data);
        }

        [Route("DeleteRecord")]
        public CollegeStudentLedgerDTO DeleteRecord([FromBody] CollegeStudentLedgerDTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           // id.FCMSGH_Id = Convert.ToInt32(HttpContext.Session.GetInt32("FCMSGH_Id"));
            return objDel.DeleteRecord(id);
        }
    }
}
