using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager.PettyCash;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager.PettyCash
{
    [Route("api/[controller]")]
    public class PC_Master_ParticularsController : Controller
    {
        PC_Master_ParticularsDelegate _del = new PC_Master_ParticularsDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("onloaddata/{id:int}")]        
        public PC_Master_ParticularsDTO onloaddata(int id)
        {
            PC_Master_ParticularsDTO data = new PC_Master_ParticularsDTO();           
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.onloaddata(data);
        }

        [Route("saverecord")]        
        public PC_Master_ParticularsDTO saverecord([FromBody] PC_Master_ParticularsDTO data)
        {            
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.saverecord(data);
        }

        [Route("deactiveY")]        
        public PC_Master_ParticularsDTO deactiveY([FromBody] PC_Master_ParticularsDTO data)
        {            
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.deactiveY(data);
        }
    }
}
