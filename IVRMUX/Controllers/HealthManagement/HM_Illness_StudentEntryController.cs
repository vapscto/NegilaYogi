using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.HealthManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.HealthManagement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.HealthManagement
{
    [Route("api/[controller]")]
    public class HM_Illness_StudentEntryController : Controller
    {
        HM_Illness_StudentEntryDelegate _delg = new HM_Illness_StudentEntryDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("LoadStudentIllnessData/{id:int}")]
        public HM_Illness_StudentEntryDTO LoadStudentIllnessData(int id)
        {
            HM_Illness_StudentEntryDTO data = new HM_Illness_StudentEntryDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.LoadStudentIllnessData(data);
        }

        [Route("SaveStudentIllnessData")]
        public HM_Illness_StudentEntryDTO SaveStudentIllnessData([FromBody] HM_Illness_StudentEntryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));            
            return _delg.SaveStudentIllnessData(data);
        }

        [Route("EditStudentIllnessData")]
        public HM_Illness_StudentEntryDTO EditStudentIllnessData([FromBody] HM_Illness_StudentEntryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));            
            return _delg.EditStudentIllnessData(data);
        }

        [Route("ActiveDeactiveStudentIllnessData")]
        public HM_Illness_StudentEntryDTO ActiveDeactiveStudentIllnessData([FromBody] HM_Illness_StudentEntryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));            
            return _delg.ActiveDeactiveStudentIllnessData(data);
        }

        [Route("GetStudentDetailsBySearch")]
        public HM_Illness_StudentEntryDTO GetStudentDetailsBySearch([FromBody] HM_Illness_StudentEntryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));            
            return _delg.GetStudentDetailsBySearch(data);
        }

        [Route("OnStudentNameChange")]
        public HM_Illness_StudentEntryDTO OnStudentNameChange([FromBody] HM_Illness_StudentEntryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));            
            return _delg.OnStudentNameChange(data);
        }

        //Student Illness Report
        [Route("LoadStudentIllnessReportData/{id:int}")]
        public HM_Illness_StudentEntryDTO LoadStudentIllnessReportData(int id)
        {
            HM_Illness_StudentEntryDTO data = new HM_Illness_StudentEntryDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.LoadStudentIllnessReportData(data);
        }

        [Route("ReportStudentIllnessData")]
        public HM_Illness_StudentEntryDTO ReportStudentIllnessData([FromBody] HM_Illness_StudentEntryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.ReportStudentIllnessData(data);
        }  

        [Route("ReportOnChangeYearData")]
        public HM_Illness_StudentEntryDTO ReportOnChangeYearData([FromBody] HM_Illness_StudentEntryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.ReportOnChangeYearData(data);
        }        
    }
}
