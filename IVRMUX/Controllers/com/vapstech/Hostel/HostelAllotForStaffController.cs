using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Hostel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Hostel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Hostel
{
    [Route("api/[controller]")]
    public class HostelAllotForStaffController : Controller
    {
        public HostelAllotForStaffDelegate _delObj = new HostelAllotForStaffDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("loaddata/{id:int}")]
        public HostelAllotForStaff_DTO loaddata(int id)
        {
            HostelAllotForStaff_DTO data = new HostelAllotForStaff_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delObj.loaddata(data);
        }

        [Route("savedata")]
        public HostelAllotForStaff_DTO savedata([FromBody] HostelAllotForStaff_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            return _delObj.savedata(data);
        }

        [Route("get_studInfo")]
        public HostelAllotForStaff_DTO get_studInfo([FromBody]HostelAllotForStaff_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delObj.get_studInfo(data);
        }

        [Route("get_roomdetails")]
        public HostelAllotForStaff_DTO get_roomdetails([FromBody]HostelAllotForStaff_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delObj.get_roomdetails(data);
        } [Route("deactivYTab1")]
        public HostelAllotForStaff_DTO deactivYTab1([FromBody]HostelAllotForStaff_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            

            return _delObj.deactivYTab1(data);
        }
        [Route("editdata")]
        public HostelAllotForStaff_DTO editdata([FromBody]HostelAllotForStaff_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            return _delObj.editdata(data);
        }
        [Route("getdesg")]
        public HostelAllotForStaff_DTO getdesg([FromBody]HostelAllotForStaff_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            return _delObj.getdesg(data);
        }
    }
}
