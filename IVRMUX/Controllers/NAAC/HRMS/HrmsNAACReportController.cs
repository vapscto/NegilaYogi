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
    public class HrmsNAACReportController : Controller
    {
        public HrmsNAACReportDelegate _delg = new HrmsNAACReportDelegate();

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

        [Route("SaveData")]
        public HRMS_NAAC_DTO SaveData([FromBody] HRMS_NAAC_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveData(data);
        }

        [Route("getOrientdata")]
        public HRMS_NAAC_DTO getOrientdata([FromBody] HRMS_NAAC_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getOrientdata(data);
        }

        [Route("getStudentActivitydata")]
        public HRMS_NAAC_DTO getStudentActivitydata([FromBody] HRMS_NAAC_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getStudentActivitydata(data);
        }

        [Route("getProfessionalActivitydata")]
        public HRMS_NAAC_DTO getProfessionalActivitydata([FromBody] HRMS_NAAC_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getProfessionalActivitydata(data);
        }

        [Route("getResearchProjectdata")]
        public HRMS_NAAC_DTO getResearchProjectdata([FromBody] HRMS_NAAC_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getResearchProjectdata(data);
        }

        [Route("getResearchGuidedata")]
        public HRMS_NAAC_DTO getResearchGuidedata([FromBody] HRMS_NAAC_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getResearchGuidedata(data);
        }

        [Route("getBOSBOEdata")]
        public HRMS_NAAC_DTO getBOSBOEdata([FromBody] HRMS_NAAC_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getBOSBOEdata(data);
        }

        [Route("getJournaldata")]
        public HRMS_NAAC_DTO getJournaldata([FromBody] HRMS_NAAC_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getJournaldata(data);
        }

        [Route("getConferencedata")]
        public HRMS_NAAC_DTO getConferencedata([FromBody] HRMS_NAAC_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getConferencedata(data);
        }

        [Route("getBookdata")]
        public HRMS_NAAC_DTO getBookdata([FromBody] HRMS_NAAC_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getBookdata(data);
        }

        [Route("getBookChapterdata")]
        public HRMS_NAAC_DTO getBookChapterdata([FromBody] HRMS_NAAC_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getBookChapterdata(data);
        }

        [Route("getCommetteedata")]
        public HRMS_NAAC_DTO getCommetteedata([FromBody] HRMS_NAAC_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getCommetteedata(data);
        }

        [Route("getOtherDetaildata")]
        public HRMS_NAAC_DTO getOtherDetaildata([FromBody] HRMS_NAAC_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getOtherDetaildata(data);
        }

        [Route("get_EmployeALLDATA")]
        public HRMS_NAAC_DTO get_EmployeALLDATA([FromBody] HRMS_NAAC_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_EmployeALLDATA(data);
        }
        
    }
}
