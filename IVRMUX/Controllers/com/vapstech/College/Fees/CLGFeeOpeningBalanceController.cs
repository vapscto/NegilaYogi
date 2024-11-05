using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.College.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Fees
{
    [Route("api/[controller]")]
    public class CLGFeeOpeningBalanceController : Controller
    {
        CLGFeeOpeningBalanceDelegate _del = new CLGFeeOpeningBalanceDelegate();
          // GET api/values/5
          [HttpGet]
        [Route("getalldetails/{id:int}")]
        public CLGFeeOpeningBalanceDTO getalldetails(int id)
        {
            CLGFeeOpeningBalanceDTO data = new CLGFeeOpeningBalanceDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.getalldetails(data);
        }

        // POST api/values
        [HttpPost]       
        [Route("get_courses")]
        public CLGFeeOpeningBalanceDTO get_courses([FromBody] CLGFeeOpeningBalanceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            return _del.get_courses(data);
        }
        [Route("get_branches")]
        public CLGFeeOpeningBalanceDTO get_branches([FromBody] CLGFeeOpeningBalanceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.get_branches(data);
        }
        [Route("get_semisters")]
        public CLGFeeOpeningBalanceDTO get_semisters([FromBody] CLGFeeOpeningBalanceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.get_semisters(data);
        }
        [Route("get_groups")]
        public CLGFeeOpeningBalanceDTO get_groups([FromBody] CLGFeeOpeningBalanceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            return _del.get_groups(data);
        }
        [Route("get_heads")]
        public CLGFeeOpeningBalanceDTO get_heads([FromBody] CLGFeeOpeningBalanceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            return _del.get_heads(data);
        }
        [Route("get_installments")]
        public CLGFeeOpeningBalanceDTO get_installments([FromBody] CLGFeeOpeningBalanceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            return _del.get_installments(data);
        }
        [Route("get_students")]
        public CLGFeeOpeningBalanceDTO get_students([FromBody] CLGFeeOpeningBalanceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.get_students(data);
        }
        [Route("savedata")]
        public CLGFeeOpeningBalanceDTO savedata([FromBody] CLGFeeOpeningBalanceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            return _del.savedata(data);
        }
        [Route("Deletedetails")]
        public CLGFeeOpeningBalanceDTO Deletedetails([FromBody] CLGFeeOpeningBalanceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.Deletedetails(data);
        }
    }
}
