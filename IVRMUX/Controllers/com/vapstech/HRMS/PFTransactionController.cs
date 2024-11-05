using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.HRMS;
using IVRMUX.Delegates.com.vapstech.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class PFTransactionController : Controller
    {
        PFTransactionDelegate del = new PFTransactionDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public PFReportsDTO getalldetails(int id)
        {
            PFReportsDTO dto = new PFReportsDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.onloadgetdetails(dto);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }


        [Route("SavePFData")]
        public PFReportsDTO SavePFData([FromBody]PFReportsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.SavePFData(dto);
        }


        [Route("getReport")]
        public PFReportsDTO getEmployeedetailsBySelectionBBKV([FromBody]PFReportsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.getReport(dto);
        }
        //FilterEmployeeData

        [Route("FilterEmployeeData")]
        public PFReportsDTO FilterEmployeeData([FromBody]PFReportsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.FilterEmployeeData(dto);
        }
        [Route("editdata")]
        public PFReportsDTO editdata([FromBody]PFReportsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.editdata(dto);
        }

        [Route("getEmployeedetailsBySelectionStJames")]
        public PFReportsDTO getEmployeedetailsBySelectionStJames([FromBody]PFReportsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.getEmployeedetailsBySelectionStJames(dto);
        }

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public PFReportsDTO getloaddata(int id)
        {
            PFReportsDTO dto = new PFReportsDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            // dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getloaddata(dto);
        }

        [Route("savedetails")]
        public PFReportsDTO savedetails([FromBody] PFReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return del.savedetails(data);
        }
        [Route("deactive")]
        public PFReportsDTO deactive([FromBody] PFReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deactive(data);
        }
        [Route("PFBlurcalculation")]
        public PFReportsDTO PFBlurcalculation([FromBody] PFReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.PFBlurcalculation(data);
        }
        [Route("EditSave")]
        public PFReportsDTO PFBlurcalEditSaveculation([FromBody] PFReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.EditSave(data);
        }
        [Route("finalverify")]
        public PFReportsDTO finalverify([FromBody] PFReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.finalverify(data);
        }
    }
}
