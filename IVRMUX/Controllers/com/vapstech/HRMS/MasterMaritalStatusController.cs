using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using corewebapi18072016.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterMaritalStatusController : Controller
    {
        MasterMaritalStatusDelegate del = new MasterMaritalStatusDelegate();
       
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public IVRM_Master_Marital_StatusDTO getalldetails(int id)
        {
            IVRM_Master_Marital_StatusDTO dto = new IVRM_Master_Marital_StatusDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.onloadgetdetails(dto);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public IVRM_Master_Marital_StatusDTO Post([FromBody]IVRM_Master_Marital_StatusDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.savedetails(dto);
        }

        [Route("editRecord/{id:int}")]
        public IVRM_Master_Marital_StatusDTO editRecord(int id)
        {
            IVRM_Master_Marital_StatusDTO dto = new IVRM_Master_Marital_StatusDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getRecorddetailsById(id);
        }

        [Route("ActiveDeactiveRecord/{id:int}")]
        public IVRM_Master_Marital_StatusDTO ActiveDeactiveRecord(int id)
        {
            IVRM_Master_Marital_StatusDTO dto = new IVRM_Master_Marital_StatusDTO();
            dto.IVRMMMS_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.deleterec(dto);
        }
    }
}
