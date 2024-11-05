using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using LeaveManagementServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace LeaveManagementServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class LeaveConfigFacade : Controller
    {
        LeaveConfigInterface _config;

        public LeaveConfigFacade(LeaveConfigInterface config)
        {
            _config = config;
        }
        // GET: api/values
        [Route("save")]
        public HR_Leave_Policy_Config_DTO Save([FromBody]HR_Leave_Policy_Config_DTO data)
        {
            return _config.save(data);
        }
        [Route("getSPName")]
        public HR_Leave_Policy_Config_DTO getSPName([FromBody]HR_Leave_Policy_Config_DTO data)
        {
            return _config.getSPName(data);
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
