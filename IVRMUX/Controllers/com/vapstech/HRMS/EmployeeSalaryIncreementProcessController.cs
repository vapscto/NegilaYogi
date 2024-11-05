using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using corewebapi18072016.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using IVRMUX.Delegates.com.vapstech.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class EmployeeSalaryIncreementProcessController : Controller
    {
        EmployeeSalaryIncreementProcessDelegate del = new EmployeeSalaryIncreementProcessDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public EmployeeSalaryIncreementProcessDTO getalldetails(int id)
        {
            EmployeeSalaryIncreementProcessDTO dto = new EmployeeSalaryIncreementProcessDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return del.onloadgetdetails(dto);
        }

        [Route("getReport")]
        public EmployeeSalaryIncreementProcessDTO getReport([FromBody]EmployeeSalaryIncreementProcessDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getReport(dto);
        }
        [Route("Empdetails")]
        public EmployeeSalaryIncreementProcessDTO Empdetails([FromBody]EmployeeSalaryIncreementProcessDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.Empdetails(dto);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public EmployeeSalaryIncreementProcessDTO Post([FromBody]EmployeeSalaryIncreementProcessDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return del.savedetails(dto);
        }

        [Route("editRecord/{id:int}")]
        public EmployeeSalaryIncreementProcessDTO editRecord(int id)
        {
            //  HR_Master_CourseDTO dto = new HR_Master_CourseDTO();
            // dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getRecorddetailsById(id);
        }

        [Route("ActiveDeactiveRecord/{id:int}")]
        public EmployeeSalaryIncreementProcessDTO ActiveDeactiveRecord(int id)
        {
            EmployeeSalaryIncreementProcessDTO dto = new EmployeeSalaryIncreementProcessDTO();
            dto.HREIC_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return del.deleterec(dto);
        }
    }
}
