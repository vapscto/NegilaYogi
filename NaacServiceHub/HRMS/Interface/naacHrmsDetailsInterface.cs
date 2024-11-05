
using PreadmissionDTOs.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.HRMS.Interface
{
    public interface naacHrmsDetailsInterface
    {

        HRMS_NAAC_DTO getdetails(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO get_depts(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO get_desig(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO get_Employe_ob(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO SaveData(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO getOrientdata(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO getStudentActivitydata(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO getProfessionalActivitydata(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO getResearchProjectdata(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO getResearchGuidedata(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO getBOSBOEdata(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO getJournaldata(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO getConferencedata(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO getBookdata(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO getBookChapterdata(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO getCommetteedata(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO getOtherDetaildata(HRMS_NAAC_DTO data);
        Task<HRMS_NAAC_DTO> get_EmployeALLDATAAsync(HRMS_NAAC_DTO data);
        HR_Employee_OrientationCourseDTO DeleteDocumentRecordOrientation(HR_Employee_OrientationCourseDTO data);
        HR_Employee_StudentActivitiesDTO DeleteDocumentRecordStuActivity(HR_Employee_StudentActivitiesDTO data);
        HR_Employee_DevActivitiesDTO DeleteDocumentRecordProfActivity(HR_Employee_DevActivitiesDTO data);
        HR_Employee_ResearchProjectsDTO DeleteDocumentRecordResProj(HR_Employee_ResearchProjectsDTO data);
        HR_Employee_ResearchGuidanceDTO DeleteDocumentRecordResGuide(HR_Employee_ResearchGuidanceDTO data);
        HR_Employee_BOSBOEDTO DeleteDocumentRecordBOSBOE(HR_Employee_BOSBOEDTO data);
        HR_Employee_JournalDTO DeleteDocumentRecordJournal(HR_Employee_JournalDTO data);
        HR_Employee_ConferenceDTO DeleteDocumentRecordConference(HR_Employee_ConferenceDTO data);
        HR_Employee_BookDTO DeleteDocumentRecordBook(HR_Employee_BookDTO data);
        HR_Employee_BookChapterDTO DeleteDocumentRecordBookChapter(HR_Employee_BookChapterDTO data);
        HR_Employee_CommitteeDTO DeleteDocumentRecordCommettee(HR_Employee_CommitteeDTO data);
        HR_Employee_OtherDetailsDTO DeleteDocumentRecordOthers(HR_Employee_OtherDetailsDTO data);
        HR_Employee_ExamDutyDTO DeleteDocumentRecordExamination(HR_Employee_ExamDutyDTO data);
        HRMS_NAAC_DTO editRecord(HRMS_NAAC_DTO data);
    }
}
