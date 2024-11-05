using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using corewebapi18072016.Delegates;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class InstituteWiseMandatorysettingsController : Controller
    {
        InstituteWiseMandatorysettingsDelegate del = new InstituteWiseMandatorysettingsDelegate();

        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public IVRM_Mandatory_Setting_IWDTO getalldetails(int id)
        {
            IVRM_Mandatory_Setting_IWDTO dto = new IVRM_Mandatory_Setting_IWDTO();
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
        public IVRM_Mandatory_Setting_IWDTO getPagedetailsBySelection([FromBody]IVRM_Mandatory_Setting_IWDTO dto)
        {
            return del.getPagedetailsBySelection(dto);
        }

        // POST api/values
        [HttpPost]
        public IVRM_Mandatory_Setting_IWDTO Post([FromBody]IVRM_Mandatory_Setting_IWDTO dto)
        {
            //   dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //   dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.savedetails(dto);
        }

        [Route("editRecord")]
        public IVRM_Mandatory_Setting_IWDTO editRecord([FromBody]IVRM_Mandatory_Setting_IWDTO dto)
        {
            return del.getRecorddetailsById(dto);
        }

        [Route("ActiveDeactiveRecord")]
        public IVRM_Mandatory_Setting_IWDTO ActiveDeactiveRecord([FromBody]IVRM_Mandatory_Setting_IWDTO dto)
        {
            // dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            // dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.deleterec(dto);
        }
    }
}
