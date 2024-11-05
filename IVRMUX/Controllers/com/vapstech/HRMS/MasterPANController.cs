using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.HRMS;
using Microsoft.AspNetCore.Http;

namespace IVRMUX.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterPANController : Controller
    {
        // GET: api/values
        HRMasterPANDelegate del = new HRMasterPANDelegate();

        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public HRMasterPANDTO getalldetails(int id)
        {
            HRMasterPANDTO dto = new HRMasterPANDTO();
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
        [Route("savedetails")]
        public HRMasterPANDTO savedetails([FromBody]HRMasterPANDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.savedetails(dto);
        }

        [Route("editRecord/{id:int}")]
        public HRMasterPANDTO editRecord(int id)
        {
            HRMasterPANDTO dto = new HRMasterPANDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getRecorddetailsById(id);
        }

        [Route("ActiveDeactiveRecord/{id:int}")]
        public HRMasterPANDTO ActiveDeactiveRecord(int id)
        {
            HRMasterPANDTO dto = new HRMasterPANDTO();
            dto.HRMPN_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.deleterec(dto);
        }
    }
}
