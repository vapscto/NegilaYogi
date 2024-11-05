using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.HRMS.Interface;
using PreadmissionDTOs.HRMS;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.HRMS.Facade
{
    [Route("api/[controller]")]
    public class HrmsNAACReportFacadeController : Controller
    {

        public HrmsNAACReportInterface _interface;

        public HrmsNAACReportFacadeController(HrmsNAACReportInterface inte)
        {
            _interface = inte;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getdetails")]
        public HRMS_NAAC_DTO getdetails([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.getdetails(data);
        }

        [Route("get_depts")]
        public HRMS_NAAC_DTO get_depts([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.get_depts(data);
        }

        [Route("get_desig")]
        public HRMS_NAAC_DTO get_desig([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.get_desig(data);
        }

        [Route("get_Employe_ob")]
        public HRMS_NAAC_DTO get_Employe_ob([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.get_Employe_ob(data);
        }

        [Route("SaveData")]
        public HRMS_NAAC_DTO SaveData([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.SaveData(data);
        }

        [Route("getOrientdata")]
        public HRMS_NAAC_DTO getOrientdata([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.getOrientdata(data);
        }

        [Route("getStudentActivitydata")]
        public HRMS_NAAC_DTO getStudentActivitydata([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.getStudentActivitydata(data);
        }

        [Route("getProfessionalActivitydata")]
        public HRMS_NAAC_DTO getProfessionalActivitydata([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.getProfessionalActivitydata(data);
        }

        [Route("getResearchProjectdata")]
        public HRMS_NAAC_DTO getResearchProjectdata([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.getResearchProjectdata(data);
        }

        [Route("getResearchGuidedata")]
        public HRMS_NAAC_DTO getResearchGuidedata([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.getResearchGuidedata(data);
        }

        [Route("getBOSBOEdata")]
        public HRMS_NAAC_DTO getBOSBOEdata([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.getBOSBOEdata(data);
        }

        [Route("getJournaldata")]
        public HRMS_NAAC_DTO getJournaldata([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.getJournaldata(data);
        }

        [Route("getConferencedata")]
        public HRMS_NAAC_DTO getConferencedata([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.getConferencedata(data);
        }

        [Route("getBookdata")]
        public HRMS_NAAC_DTO getBookdata([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.getBookdata(data);
        }

        [Route("getBookChapterdata")]
        public HRMS_NAAC_DTO getBookChapterdata([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.getBookChapterdata(data);
        }

        [Route("getCommetteedata")]
        public HRMS_NAAC_DTO getCommetteedata([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.getCommetteedata(data);
        }

        [Route("getOtherDetaildata")]
        public HRMS_NAAC_DTO getOtherDetaildata([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.getOtherDetaildata(data);
        }

        [Route("get_EmployeALLDATA")]
        public HRMS_NAAC_DTO get_EmployeALLDATA([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.get_EmployeALLDATA(data);
        }
        
    }
}
