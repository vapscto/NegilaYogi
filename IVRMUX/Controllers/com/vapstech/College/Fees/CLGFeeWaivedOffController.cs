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
    public class CLGFeeWaivedOffController : Controller
    {
        CLGFeeWaivedOffDelegate _del = new CLGFeeWaivedOffDelegate();

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public CLGFeeWaivedOffDTO getalldetails(int id)
        {
            CLGFeeWaivedOffDTO data = new CLGFeeWaivedOffDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;
            return _del.getalldetails(data);
        }
        [Route("EditDetails/{id:int}")]
        public CLGFeeWaivedOffDTO EditDetails(int id)
        {
            CLGFeeWaivedOffDTO data = new CLGFeeWaivedOffDTO();
            data.FCSWO_Id = id;
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            return _del.EditDetails(data);
           
        }

        [HttpPost]
        [Route("get_students")]
        public CLGFeeWaivedOffDTO get_students([FromBody] CLGFeeWaivedOffDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            return _del.get_students(data);
        }
        [Route("get_groups")]
        public CLGFeeWaivedOffDTO get_groups([FromBody] CLGFeeWaivedOffDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            return _del.get_groups(data);
        }
        [Route("get_heads")]
        public CLGFeeWaivedOffDTO get_heads([FromBody] CLGFeeWaivedOffDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            return _del.get_heads(data);
        }
        [Route("savedata")]
        public CLGFeeWaivedOffDTO savedata([FromBody] CLGFeeWaivedOffDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            return _del.savedata(data);
        }
        [HttpDelete]
        [Route("DeletRecord/{id:int}")]
        public CLGFeeWaivedOffDTO DeletRecord(int id)
        {
            CLGFeeWaivedOffDTO data = new CLGFeeWaivedOffDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.FCSWO_Id = id;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            return _del.DeletRecord(data);
        }
    }
}
