using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Fees;
using corewebapi18072016.Delegates.com.vapstech.Fees;
using PreadmissionDTOs.com.vaps.admission;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    public class FeeStudentConcessionReportController : Controller
    {

        FeeStudentConcessionReportDelegate feestuconnectionreport = new FeeStudentConcessionReportDelegate();
       
     
       
       
        [Route("getalldetails123")]
        public StudentConcesstionDTO Get123([FromBody] StudentConcesstionDTO data)
        {
            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.asmyid = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return feestuconnectionreport.getdata123(data);
        }



        //  POST api/values

        [HttpPost]
        [Route("getreport")]
        public StudentConcesstionDTO getreport([FromBody] StudentConcesstionDTO data123)
        {
            //StudentConcesstionDTO data = new StudentConcesstionDTO();
            data123.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id")); 
            return feestuconnectionreport.getreport(data123);
        }
        [Route("get_groups")]
        public FeeStudentTransactionDTO get_groups([FromBody]FeeStudentTransactionDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //value.asmyid = ASMAY_Id;

            return feestuconnectionreport.get_groups(value);
        }
    }
}
