using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class GroupDeptDessgController : Controller
    {
        GroupDeptDessgDelegate del = new GroupDeptDessgDelegate();
        // GET: api/values
        [HttpGet]
        [Route("loaddata/{id:int}")]
        public HRGroupDeptDessgDTO loaddata(int id)
        {
            HRGroupDeptDessgDTO dto = new HRGroupDeptDessgDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loaddata(dto);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("savedata")]
        public HRGroupDeptDessgDTO savedata([FromBody]HRGroupDeptDessgDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedata(dto);
        }


        [Route("Editdata")]
        public HRGroupDeptDessgDTO Editdata([FromBody]HRGroupDeptDessgDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.Editdata(dto);
        }
        [Route("masterDecative")]
        public HRGroupDeptDessgDTO masterDecative([FromBody]HRGroupDeptDessgDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           // dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.masterDecative(dto);
        }
    }
}
