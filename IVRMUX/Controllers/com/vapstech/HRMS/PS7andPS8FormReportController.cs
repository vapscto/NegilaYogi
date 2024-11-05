using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.HRMS;
using Microsoft.AspNetCore.Http;
using IVRMUX.Delegates.com.vapstech.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class PS7andPS8FormReportController : Controller
    {
        // GET: api/values
        PS7andPS8FormReportDelegate del = new PS7andPS8FormReportDelegate();
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

        //getdata for ps7
        [Route("getEmployeedetailsBySelection")]
        public PFReportsDTO getEmployeedetailsBySelection([FromBody]PFReportsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.getEmployeedetailsBySelection(dto);
        }

        //getdata for ps8

        [Route("getdataps8")]
        public PFReportsDTO getdataps8([FromBody]PFReportsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.getdataps8(dto);
        }
    }
}
