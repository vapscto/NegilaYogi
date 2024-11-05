using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using corewebapi18072016.Delegates.com.vapstech.LeaveManagement;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.LeaveManagement
{
    [Route("api/[controller]")]
    public class LeaveConfigController : Controller
    {
        LeaveConfigDelegate config = new LeaveConfigDelegate();
        // GET: api/values
        [Route("getSPName/{id:int}")]

        public HR_Leave_Policy_Config_DTO getSPName(int id)
        {
            HR_Leave_Policy_Config_DTO mid = new HR_Leave_Policy_Config_DTO();
            mid.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return config.getSPName(mid);
        }
        // POST api/values

        [Route("SaveData")]
        public HR_Leave_Policy_Config_DTO SaveData([FromBody]HR_Leave_Policy_Config_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return config.savedata(data);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
