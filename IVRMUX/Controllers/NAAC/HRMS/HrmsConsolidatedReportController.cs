using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.HRMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.HRMS
{
    [Route("api/[controller]")]
    public class HrmsConsolidatedReportController : Controller
    {
        public HrmsConsolidatedReportDelegate _delg = new HrmsConsolidatedReportDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet]
        [Route("getdetails/{id:int}")]
        public HRMS_NAAC_DTO getdetails(int id)
        {
            HRMS_NAAC_DTO dto = new HRMS_NAAC_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getdetails(dto);
        }

        [Route("get_depts")]
        public HRMS_NAAC_DTO get_depts([FromBody] HRMS_NAAC_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_depts(data);
        }

        [Route("get_desig")]
        public HRMS_NAAC_DTO get_desig([FromBody] HRMS_NAAC_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_desig(data);
        }

        [Route("get_Employe_ob")]
        public HRMS_NAAC_DTO get_Employe_ob([FromBody] HRMS_NAAC_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_Employe_ob(data);
        }

        [Route("getEmployeReport")]
        public HRMS_NAAC_DTO getEmployeReport([FromBody] HRMS_NAAC_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getEmployeReport(data);
        }

        [Route("get_EmployeALLDATA")]
        public HRMS_NAAC_DTO get_EmployeALLDATA([FromBody] HRMS_NAAC_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_EmployeALLDATA(data);
        }
        
    }
}
