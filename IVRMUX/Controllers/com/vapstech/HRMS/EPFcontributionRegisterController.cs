using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class EPFcontributionRegisterController : Controller
    {
        EPFcontributionRegisterDelegate del = new EPFcontributionRegisterDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public PFReportsDTO getalldetails(int id)
        {
          PFReportsDTO dto = new PFReportsDTO();
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


        [Route("getEmployeedetailsBySelection")]
        public PFReportsDTO getEmployeedetailsBySelection([FromBody]PFReportsDTO dto)
        {
          dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
          return del.getEmployeedetailsBySelection(dto);
        }


        [Route("getEmployeedetailsBySelectionBBKV")]
        public PFReportsDTO getEmployeedetailsBySelectionBBKV([FromBody]PFReportsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.getEmployeedetailsBySelectionBBKV(dto);
        }
        //FilterEmployeeData

        [Route("FilterEmployeeData")]
        public PFReportsDTO FilterEmployeeData([FromBody]PFReportsDTO dto)
        {
          dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
          return del.FilterEmployeeData(dto);
        }

        [Route("getEmployeedetailsBySelectionStJames")]
        public PFReportsDTO getEmployeedetailsBySelectionStJames([FromBody]PFReportsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.getEmployeedetailsBySelectionStJames(dto);
        }
    }
}
