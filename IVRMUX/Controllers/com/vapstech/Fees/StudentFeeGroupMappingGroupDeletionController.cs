using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]

    [Route("api/[controller]")]
    public class StudentFeeGroupMappingGroupDeletionController : Controller
    {
        StudentFeeGroupMappingGroupDeletionDelegate od = new StudentFeeGroupMappingGroupDeletionDelegate();
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeStudentGroupMappingDTO Get(int id)
        {
            FeeStudentGroupMappingDTO pgmodu = new FeeStudentGroupMappingDTO();
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.getdata(pgmodu);
        }
        [Route("Deletedetails")]
        public FeeStudentGroupMappingDTO Delete([FromBody] FeeStudentGroupMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.deleterecord(data);
        }
        [Route("Getreport")]
        public FeeStudentGroupMappingDTO Getreport([FromBody] FeeStudentGroupMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.Getreport(data);
        }
        [Route("onclickClass")]
        public FeeStudentGroupMappingDTO onclickClass([FromBody] FeeStudentGroupMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.onclickClass(data);
        }
    }
}
