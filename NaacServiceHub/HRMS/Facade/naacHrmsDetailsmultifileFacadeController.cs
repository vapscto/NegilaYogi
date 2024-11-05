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
    public class naacHrmsDetailsmultifileFacadeController : Controller
    {
        public naacHrmsDetailsmultifileInterface _interface;

        public naacHrmsDetailsmultifileFacadeController(naacHrmsDetailsmultifileInterface inte)
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

        [Route("viewfileremark")]
        public HRMS_NAAC_DTO viewfileremark([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.viewfileremark(data);
        }

        [Route("viewsubfileremark")]
        public HRMS_NAAC_DTO viewsubfileremark([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.viewsubfileremark(data);
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
        public Task<HRMS_NAAC_DTO> get_EmployeALLDATA([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.get_EmployeALLDATAAsync(data);
        }

        [Route("DeleteDocumentRecordOrientation")]
        public HR_Employee_OrientationCourseDTO DeleteDocumentRecordOrientation([FromBody] HR_Employee_OrientationCourseDTO data)
        {
            return _interface.DeleteDocumentRecordOrientation(data);
        }

        [Route("DeleteDocumentRecordStuActivity")]
        public HR_Employee_StudentActivitiesDTO DeleteDocumentRecordStuActivity([FromBody] HR_Employee_StudentActivitiesDTO data)
        {
            return _interface.DeleteDocumentRecordStuActivity(data);
        }

        [Route("DeleteDocumentRecordProfActivity")]
        public HR_Employee_DevActivitiesDTO DeleteDocumentRecordProfActivity([FromBody] HR_Employee_DevActivitiesDTO data)
        {
            return _interface.DeleteDocumentRecordProfActivity(data);
        }

        [Route("DeleteDocumentRecordResProj")]
        public HR_Employee_ResearchProjectsDTO DeleteDocumentRecordResProj([FromBody] HR_Employee_ResearchProjectsDTO data)
        {
            return _interface.DeleteDocumentRecordResProj(data);
        }

        [Route("DeleteDocumentRecordResGuide")]
        public HR_Employee_ResearchGuidanceDTO DeleteDocumentRecordResGuide([FromBody] HR_Employee_ResearchGuidanceDTO data)
        {
            return _interface.DeleteDocumentRecordResGuide(data);
        }

        [Route("DeleteDocumentRecordBOSBOE")]
        public HR_Employee_BOSBOEDTO DeleteDocumentRecordBOSBOE([FromBody] HR_Employee_BOSBOEDTO data)
        {
            return _interface.DeleteDocumentRecordBOSBOE(data);
        }

        [Route("DeleteDocumentRecordJournal")]
        public HR_Employee_JournalDTO DeleteDocumentRecordJournal([FromBody] HR_Employee_JournalDTO data)
        {
            return _interface.DeleteDocumentRecordJournal(data);
        }

        [Route("DeleteDocumentRecordConference")]
        public HR_Employee_ConferenceDTO DeleteDocumentRecordConference([FromBody] HR_Employee_ConferenceDTO data)
        {
            return _interface.DeleteDocumentRecordConference(data);
        }

        [Route("DeleteDocumentRecordBook")]
        public HR_Employee_BookDTO DeleteDocumentRecordBook([FromBody] HR_Employee_BookDTO data)
        {
            return _interface.DeleteDocumentRecordBook(data);
        }

        [Route("DeleteDocumentRecordBookChapter")]
        public HR_Employee_BookChapterDTO DeleteDocumentRecordBookChapter([FromBody] HR_Employee_BookChapterDTO data)
        {
            return _interface.DeleteDocumentRecordBookChapter(data);
        }

        [Route("DeleteDocumentRecordCommettee")]
        public HR_Employee_CommitteeDTO DeleteDocumentRecordCommettee([FromBody] HR_Employee_CommitteeDTO data)
        {
            return _interface.DeleteDocumentRecordCommettee(data);
        }

        [Route("DeleteDocumentRecordOthers")]
        public HR_Employee_OtherDetailsDTO DeleteDocumentRecordOthers([FromBody] HR_Employee_OtherDetailsDTO data)
        {
            return _interface.DeleteDocumentRecordOthers(data);
        }

        [Route("DeleteDocumentRecordExamination")]
        public HR_Employee_ExamDutyDTO DeleteDocumentRecordExamination([FromBody] HR_Employee_ExamDutyDTO data)
        {
            return _interface.DeleteDocumentRecordExamination(data);
        }

        [Route("editRecord")]
        public HRMS_NAAC_DTO editRecord([FromBody] HRMS_NAAC_DTO data)
        {
            return _interface.editRecord(data);
        }
    }
}
