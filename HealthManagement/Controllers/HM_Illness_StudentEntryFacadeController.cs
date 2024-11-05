using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthManagement.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.HealthManagement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HealthManagement.Controllers
{
    [Route("api/[controller]")]
    public class HM_Illness_StudentEntryFacadeController : Controller
    {
        HM_Illness_StudentEntryInterface _interface;

        public HM_Illness_StudentEntryFacadeController(HM_Illness_StudentEntryInterface _inter)
        {
            _interface = _inter;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("LoadStudentIllnessData")]
        public HM_Illness_StudentEntryDTO LoadStudentIllnessData([FromBody] HM_Illness_StudentEntryDTO data)
        {
            return _interface.LoadStudentIllnessData(data);
        }

        [Route("SaveStudentIllnessData")]
        public HM_Illness_StudentEntryDTO SaveStudentIllnessData([FromBody] HM_Illness_StudentEntryDTO data)
        {
            return _interface.SaveStudentIllnessData(data);
        }

        [Route("EditStudentIllnessData")]
        public HM_Illness_StudentEntryDTO EditStudentIllnessData([FromBody] HM_Illness_StudentEntryDTO data)
        {
            return _interface.EditStudentIllnessData(data);
        }

        [Route("ActiveDeactiveStudentIllnessData")]
        public HM_Illness_StudentEntryDTO ActiveDeactiveStudentIllnessData([FromBody] HM_Illness_StudentEntryDTO data)
        {
            return _interface.ActiveDeactiveStudentIllnessData(data);
        }

        [Route("GetStudentDetailsBySearch")]
        public HM_Illness_StudentEntryDTO GetStudentDetailsBySearch([FromBody] HM_Illness_StudentEntryDTO data)
        {
            return _interface.GetStudentDetailsBySearch(data);
        }

        [Route("OnStudentNameChange")]
        public HM_Illness_StudentEntryDTO OnStudentNameChange([FromBody] HM_Illness_StudentEntryDTO data)
        {
            return _interface.OnStudentNameChange(data);
        }

        // Student Illness Report
        [Route("LoadStudentIllnessReportData")]
        public HM_Illness_StudentEntryDTO LoadStudentIllnessReportData([FromBody] HM_Illness_StudentEntryDTO data)
        {
            return _interface.LoadStudentIllnessReportData(data);
        }

        [Route("ReportStudentIllnessData")]
        public HM_Illness_StudentEntryDTO ReportStudentIllnessData([FromBody] HM_Illness_StudentEntryDTO data)
        {
            return _interface.ReportStudentIllnessData(data);
        }

        [Route("ReportOnChangeYearData")]
        public HM_Illness_StudentEntryDTO ReportOnChangeYearData([FromBody] HM_Illness_StudentEntryDTO data)
        {
            return _interface.ReportOnChangeYearData(data);
        }
    }
}
