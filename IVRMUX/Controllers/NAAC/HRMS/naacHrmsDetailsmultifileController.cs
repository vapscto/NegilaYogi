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
    public class naacHrmsDetailsmultifileController : Controller
    {
        public naacHrmsDetailsmultifileDelegate _delg = new naacHrmsDetailsmultifileDelegate();

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

        [Route("viewfileremark")]
        public HRMS_NAAC_DTO viewfileremark([FromBody] HRMS_NAAC_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.viewfileremark(data);
        }

        [Route("viewsubfileremark")]
        public HRMS_NAAC_DTO viewsubfileremark([FromBody] HRMS_NAAC_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.viewsubfileremark(data);
        }

        [Route("SaveData")]
        public HRMS_NAAC_DTO SaveData([FromBody] HRMS_NAAC_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveData(data);
        }

        [Route("empSaveData")]
        public HRMS_NAAC_DTO empSaveData([FromBody] HRMS_NAAC_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.HRME_Id = 21;
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

        [Route("empget_EmployeALLDATA")]
        public HRMS_NAAC_DTO empget_EmployeALLDATA([FromBody] HRMS_NAAC_DTO data)
        {
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.HRME_Id = 21;
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_EmployeALLDATA(data);
        }

        [Route("DeleteDocumentRecordOrientation")]
        public HR_Employee_OrientationCourseDTO DeleteDocumentRecordOrientation([FromBody] HR_Employee_OrientationCourseDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.DeleteDocumentRecordOrientation(data);
        }

        [Route("DeleteDocumentRecordStuActivity")]
        public HR_Employee_StudentActivitiesDTO DeleteDocumentRecordStuActivity([FromBody] HR_Employee_StudentActivitiesDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.DeleteDocumentRecordStuActivity(data);
        }

        [Route("DeleteDocumentRecordProfActivity")]
        public HR_Employee_DevActivitiesDTO DeleteDocumentRecordProfActivity([FromBody] HR_Employee_DevActivitiesDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.DeleteDocumentRecordProfActivity(data);
        }

        [Route("DeleteDocumentRecordResProj")]
        public HR_Employee_ResearchProjectsDTO DeleteDocumentRecordResProj([FromBody] HR_Employee_ResearchProjectsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.DeleteDocumentRecordResProj(data);
        }

        [Route("DeleteDocumentRecordResGuide")]
        public HR_Employee_ResearchGuidanceDTO DeleteDocumentRecordResGuide([FromBody] HR_Employee_ResearchGuidanceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.DeleteDocumentRecordResGuide(data);
        }

        [Route("DeleteDocumentRecordBOSBOE")]
        public HR_Employee_BOSBOEDTO DeleteDocumentRecordBOSBOE([FromBody] HR_Employee_BOSBOEDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.DeleteDocumentRecordBOSBOE(data);
        }

        [Route("DeleteDocumentRecordJournal")]
        public HR_Employee_JournalDTO DeleteDocumentRecordJournal([FromBody] HR_Employee_JournalDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.DeleteDocumentRecordJournal(data);
        }

        [Route("DeleteDocumentRecordConference")]
        public HR_Employee_ConferenceDTO DeleteDocumentRecordConference([FromBody] HR_Employee_ConferenceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.DeleteDocumentRecordConference(data);
        }

        [Route("DeleteDocumentRecordBook")]
        public HR_Employee_BookDTO DeleteDocumentRecordBook([FromBody] HR_Employee_BookDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.DeleteDocumentRecordBook(data);
        }

        [Route("DeleteDocumentRecordBookChapter")]
        public HR_Employee_BookChapterDTO DeleteDocumentRecordBookChapter([FromBody] HR_Employee_BookChapterDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.DeleteDocumentRecordBookChapter(data);
        }

        [Route("DeleteDocumentRecordCommettee")]
        public HR_Employee_CommitteeDTO DeleteDocumentRecordCommettee([FromBody] HR_Employee_CommitteeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.DeleteDocumentRecordCommettee(data);
        }

        [Route("DeleteDocumentRecordOthers")]
        public HR_Employee_OtherDetailsDTO DeleteDocumentRecordOthers([FromBody] HR_Employee_OtherDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.DeleteDocumentRecordOthers(data);
        }

        [Route("DeleteDocumentRecordExamination")]
        public HR_Employee_ExamDutyDTO DeleteDocumentRecordExamination([FromBody] HR_Employee_ExamDutyDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.DeleteDocumentRecordExamination(data);
        }

        [Route("editRecord")]
        public HRMS_NAAC_DTO editRecord([FromBody] HRMS_NAAC_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.editRecord(data);
        }
    }
}
