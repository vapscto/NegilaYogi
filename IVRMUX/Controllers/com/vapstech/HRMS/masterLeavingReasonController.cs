using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.HRMS
{
    [Route("api/[controller]")]
    public class masterLeavingReasonController : Controller
    {
        masterLeavingReasonDelegate del = new masterLeavingReasonDelegate();

        // GET: api/<controller>
        [HttpGet]
        [Route("loaddata/{id:int}")]
        public masterLeavingReasonDTO loaddata(int id)
        {
            masterLeavingReasonDTO data = new masterLeavingReasonDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }
        [Route("savedata")]
        public masterLeavingReasonDTO savedata([FromBody]masterLeavingReasonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedata(data);
        }
        [Route("EditData")]
        public masterLeavingReasonDTO EditData([FromBody]masterLeavingReasonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }
        [Route("masterDecative")]
        public masterLeavingReasonDTO masterDecative([FromBody]masterLeavingReasonDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.masterDecative(data);
        }
    }
}
