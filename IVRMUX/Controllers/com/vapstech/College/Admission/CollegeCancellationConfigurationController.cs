using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using corewebapi18072016.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;
using IVRMUX.Delegates.com.vapstech.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class CollegeCancellationConfigurationController : Controller
    {
        CollegeCancellationConfigurationDelegate _sem = new CollegeCancellationConfigurationDelegate();
        [HttpGet]
        public CollegeCancellationConfigurationDTO getdata(int id)
        {
            CollegeCancellationConfigurationDTO data = new CollegeCancellationConfigurationDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _sem.getdata(data);
        }
        [Route("saveconfig")]
        public CollegeCancellationConfigurationDTO saveconfig([FromBody]CollegeCancellationConfigurationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ACACC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _sem.saveconfig(data);
        }
        [Route("editconfig")]
        public CollegeCancellationConfigurationDTO editconfig([FromBody]CollegeCancellationConfigurationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _sem.editconfig(data);
        }
        [Route("activedeactive")]
        public CollegeCancellationConfigurationDTO activedeactive([FromBody]CollegeCancellationConfigurationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ACACC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _sem.activedeactive(data);
        }

    }
}
