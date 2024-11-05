using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MandatorysettingsController : Controller
    {
        MandatorysettingsDelegate del = new MandatorysettingsDelegate();

        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public IVRM_Mandatory_SettingDTO getalldetails(int id)
        {
            IVRM_Mandatory_SettingDTO dto = new IVRM_Mandatory_SettingDTO();
           // dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           // dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.onloadgetdetails(dto);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("getPagedetailsBySelection")]
        public IVRM_Mandatory_SettingDTO getPagedetailsBySelection([FromBody]IVRM_Mandatory_SettingDTO dto)
        {
            return del.getPagedetailsBySelection(dto);
        }

        // POST api/values
        [HttpPost]
        public IVRM_Mandatory_SettingDTO Post([FromBody]IVRM_Mandatory_SettingDTO dto)
        {
         //   dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
         //   dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.savedetails(dto);
        }

        [Route("editRecord/{id:int}")]
        public IVRM_Mandatory_SettingDTO editRecord(int id)
        {
            IVRM_Mandatory_SettingDTO dto = new IVRM_Mandatory_SettingDTO();
           // dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getRecorddetailsById(id);
        }

        [Route("ActiveDeactiveRecord/{id:int}")]
        public IVRM_Mandatory_SettingDTO ActiveDeactiveRecord(int id)
        {
            IVRM_Mandatory_SettingDTO dto = new IVRM_Mandatory_SettingDTO();
            dto.IVRMP_Id = id;
           // dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           // dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.deleterec(dto);
        }
    }
}
