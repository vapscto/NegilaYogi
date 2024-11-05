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
    public class HostelAllotForStudentController : Controller
    {
        public HostelAllotForStudentDelegate _delObj = new HostelAllotForStudentDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("loaddata/{id:int}")]
        public HostelAllotForStudent_DTO loaddata(int id)
        {
            HostelAllotForStudent_DTO data = new HostelAllotForStudent_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delObj.loaddata(data);
        }

        [Route("savedata")]
        public HostelAllotForStudent_DTO savedata([FromBody] HostelAllotForStudent_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));            

            return _delObj.savedata(data);
        }

        [Route("get_studInfo")]
        public HostelAllotForStudent_DTO get_studInfo([FromBody]HostelAllotForStudent_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delObj.get_studInfo(data);
        }

        [Route("get_roomdetails")]
        public HostelAllotForStudent_DTO get_roomdetails([FromBody]HostelAllotForStudent_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delObj.get_roomdetails(data);
        }
        [Route("editdata")]
        public HostelAllotForStudent_DTO editdata([FromBody]HostelAllotForStudent_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));            

            return _delObj.editdata(data);
        }
    }
}
