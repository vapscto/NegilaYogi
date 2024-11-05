using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Fees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    public class StudentFeeGroupMappingNextAcaYrController : Controller
    {
        StudentFeeGroupMappingNextAcaYrDelegate od = new StudentFeeGroupMappingNextAcaYrDelegate();
        // GET: api/<controller>

        [Route("loaddata")]
        public FeeStudentGroupMappingDTO getinitialdropdowns([FromBody] FeeStudentGroupMappingDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return od.getdata(data);
        }

        [Route("getgroupmappedheads")]
        public FeeStudentGroupMappingDTO getgroupmappedheads([FromBody] FeeStudentGroupMappingDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            return od.getgroupmappedheads(data);
        }

        [Route("savedata")]
        public FeeStudentGroupMappingDTO savedata([FromBody] FeeStudentGroupMappingDTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.savedetails(pgmodu);
        }

        [Route("searching")]
        public FeeStudentGroupMappingDTO searching([FromBody] FeeStudentGroupMappingDTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.searching(pgmodu);
        }


        [Route("Deletedetails")]
        public FeeStudentGroupMappingDTO Deletedetails([FromBody] FeeStudentGroupMappingDTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.Deletedetails(pgmodu);
        }

    }
}
