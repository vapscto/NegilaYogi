using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Fees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Fees
{
    [Route("api/[controller]")]
    public class CLGFeeChequeBounceController : Controller
    {
        CLGFeeChequeBounceDelegate _del = new CLGFeeChequeBounceDelegate();

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public CLGFeeChequeBounceDTO getalldetails(int id)
        {
            CLGFeeChequeBounceDTO data = new CLGFeeChequeBounceDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            return _del.getalldetails(data);
        }

        [HttpPost]
        [Route("get_students")]
        public CLGFeeChequeBounceDTO get_students([FromBody] CLGFeeChequeBounceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            return _del.get_students(data);
        }
        [Route("get_receipts")]
        public CLGFeeChequeBounceDTO get_receipts([FromBody] CLGFeeChequeBounceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            return _del.get_receipts(data);
        }
        [Route("savedata")]
        public CLGFeeChequeBounceDTO savedata([FromBody] CLGFeeChequeBounceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            return _del.savedata(data);
        }
        [HttpDelete]
        [Route("DeletRecord/{id:int}")]
        public CLGFeeChequeBounceDTO DeletRecord(int id)
        {
            CLGFeeChequeBounceDTO data = new CLGFeeChequeBounceDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.FCCB_Id = id;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            return _del.DeletRecord(data);
        }
    }
}
