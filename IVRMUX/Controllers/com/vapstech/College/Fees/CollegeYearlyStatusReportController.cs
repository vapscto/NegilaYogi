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
    public class CollegeYearlyStatusReportController : Controller
    {
       
        // GET: api/values
        public CollegeYearlyStatusReportDelegate objDel = new CollegeYearlyStatusReportDelegate();

        [HttpGet]
        [Route("GetYearList/{id:int}")]
        public CollegeYearlyStatusReportDTO GetYearList(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.GetYearList(id);
        }
        

       
      
      

        [Route("savedata")]
        public CollegeYearlyStatusReportDTO savedata([FromBody] CollegeYearlyStatusReportDTO pgmodu)
        {
            
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objDel.savedata(pgmodu);
        }


        [Route("get_group")]
        public CollegeYearlyStatusReportDTO get_group([FromBody] CollegeYearlyStatusReportDTO pgmodu)
        {

            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //pgmodu.MI_Id = 2;
            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objDel.get_group(pgmodu);
        }

        
    }
}
