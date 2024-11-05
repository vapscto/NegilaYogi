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
    public class Clg_StudentFeeGroupMappingController : Controller
    {
       
        // GET: api/values
        public Clg_StudentFeeGroupMappingDelegate objDel = new Clg_StudentFeeGroupMappingDelegate();

        [HttpGet]
        [Route("GetYearList/{id:int}")]
        public Clg_StudentFeeGroupMapping_DTO GetYearList(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.GetYearList(id);
        }
        [HttpPost]
        [Route("get_courses")]
        public Clg_StudentFeeGroupMapping_DTO get_courses([FromBody]Clg_StudentFeeGroupMapping_DTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.get_courses(id);
        }

        //[HttpPost]
        [Route("get_branches")]
        public Clg_StudentFeeGroupMapping_DTO get_branches([FromBody]Clg_StudentFeeGroupMapping_DTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.get_branches(id);
        }
        //[HttpPost]
        [Route("get_semisters")]
        public Clg_StudentFeeGroupMapping_DTO get_semisters([FromBody]Clg_StudentFeeGroupMapping_DTO Data)
        {
            Data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.get_semisters(Data);
        }
      //[HttpPost]
        [Route("get_report")]
        public Clg_StudentFeeGroupMapping_DTO get_report([FromBody]Clg_StudentFeeGroupMapping_DTO Data)
        {
            Data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            Data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objDel.get_report(Data);
        }

        [Route("savedata")]
        public Clg_StudentFeeGroupMapping_DTO savedata([FromBody] Clg_StudentFeeGroupMapping_DTO pgmodu)
        {
            
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //pgmodu.MI_Id = 2;
            //pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objDel.savedata(pgmodu);
        }

      [Route("editdata")]
      public Clg_StudentFeeGroupMapping_DTO editdata([FromBody]Clg_StudentFeeGroupMapping_DTO data)
        {
            data.MI_Id =Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          
            return objDel.editdata(data);
        }

        [Route("DeleteRecord")]
        public Clg_StudentFeeGroupMapping_DTO DeleteRecord([FromBody] Clg_StudentFeeGroupMapping_DTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           // id.FCMSGH_Id = Convert.ToInt32(HttpContext.Session.GetInt32("FCMSGH_Id"));
            return objDel.DeleteRecord(id);
        }
        //saveeditdata
        [Route("saveeditdata")]
        public Clg_StudentFeeGroupMapping_DTO saveeditdata([FromBody] Clg_StudentFeeGroupMapping_DTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objDel.saveeditdata(pgmodu);
        }
    }
}
