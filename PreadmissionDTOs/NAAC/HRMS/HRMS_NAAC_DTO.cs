using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.HRMS
{
    public class HRMS_NAAC_DTO
    {
        public long roleId { get; set; }
        public long LogInUserId { get; set; }
        public long HRMGT_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string retrunMsg { get; set; }
        public string TabName { get; set; }

        public long HREORCO_Id { get; set; }
        public long HREEXDT_Id { get; set; }
        public long HRESACT_Id { get; set; }
        public long HREDACT_Id { get; set; }
        public long HREREPR_Id { get; set; }
        public long HREREGU_Id { get; set; }
        public long HREBOS_Id { get; set; }
        public long HREJORNL_Id { get; set; }
        public long HRECONF_Id { get; set; }
        public long HREBK_Id { get; set; }
        public long HREBKCP_Id { get; set; }
        public long HRECOM_Id { get; set; }
        public long HREOTHDET_Id { get; set; }
        public long NCHREBOSF_Id { get; set; }
        public Array orientlistedit { get; set; }
        public Array StudentActivitylistedit { get; set; }
        public Array ProfessionalActivitylistedit { get; set; }
        public Array ResearchProjectlistedit { get; set; }
        public Array ResearchGuidelistedit { get; set; }
        public Array BOSBOElistedit { get; set; }
        public Array Journallistedit { get; set; }
        public Array RefJournallistedit { get; set; }
        public Array NonRefJournallistedit { get; set; }
        public Array Conferencelistedit { get; set; }
        public Array Booklistedit { get; set; }
        public Array BookChapterlistedit { get; set; }
        public Array Commetteelistedit { get; set; }
        public Array OtherDetailSlistedit { get; set; }
        public Array examinationlistedit { get; set; }

        public Array BOSBOEfilelistedit { get; set; }

        public Array groupTypedropdown { get; set; }
        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }
        public Array academicyearlist { get; set; }
        public Array get_emp { get; set; }
        public Array orientlist { get; set; }
        public Array StudentActivitylist { get; set; }
        public Array ProfessionalActivitylist { get; set; }
        public Array ResearchProjectlist { get; set; }
        public Array ResearchGuidelist { get; set; }
        public Array BOSBOElist { get; set; }
        public Array Journallist { get; set; }
        public Array RefJournallist { get; set; }
        public Array NonRefJournallist { get; set; }
        public Array Conferencelist { get; set; }
        public Array Booklist { get; set; }
        public Array BookChapterlist { get; set; }
        public Array Commetteelist { get; set; }
        public Array OtherDetailSlist { get; set; }
        public Array personaldetails { get; set; }
        public Array qualificationdetails { get; set; }
        public Array GroupAExamlist { get; set; }
        public Array GroupBExamlist { get; set; }
        public Array leaveyear { get; set; }
        public Array DocumentDetails { get; set; }
        public Array QualifctionDetails { get; set; }
        public Array examinationlist { get; set; }
        public Array documentCommentlist { get; set; }
        public Array documentsubCommentlist { get; set; }

        public long internatiolnalcount { get; set; }
        public long nationalcount { get; set; }
        public long nonrefjoucount { get; set; }
        public long patentcount { get; set; }
        public long bookcount { get; set; }
        public long bookchaptercount { get; set; }
        public long citationcount { get; set; }
        public long hindexcount { get; set; }
        public long itenindexcount { get; set; }
        public string selectedEmployee { get; set; }
        public long HRMLY_Id { get; set; }
        public string dataoption { get; set; }
        public string Type { get; set; }
        //public string HREMGAE_GPFlg { get; set; }
        //public string HREMGAE_Marks { get; set; }
        //public string HRMEGA_GroupAExamName { get; set; }
        //public long HREMGAE_Year { get; set; }
        //public string hremgaE_SubjectName { get; set; }
        public DateTime? assfromdate { get; set; }
        public DateTime? assessmentto { get; set; }


        public Array employeereport { get; set; }
        public long?[] hrmgT_IdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public HRMS_NAAC_DTO[] emptypes { get; set; }
        public HRMS_NAAC_DTO[] empdept { get; set; }
        public HRMS_NAAC_DTO[] empdesg { get; set; }

        public HR_Employee_OrientationCourseDTO HR_Employee_OrientationCourseDTO { get; set; }
        public HR_Employee_OrientationCourseDTO[] HR_Employee_OrientationCourseArrayDTO { get; set; }
        public HR_Employee_StudentActivitiesDTO HR_Employee_StudentActivitiesDTO { get; set; }
        public HR_Employee_StudentActivitiesDTO[] HR_Employee_StudentActivitiesArrayDTO { get; set; }
        public HR_Employee_ResearchProjectsDTO HR_Employee_ResearchProjectsDTO { get; set; }
        public HR_Employee_ResearchProjectsDTO[] HR_Employee_ResearchProjectsArrayDTO { get; set; }
        public HR_Employee_ResearchGuidanceDTO HR_Employee_ResearchGuidanceDTO { get; set; }
        public HR_Employee_ResearchGuidanceDTO[] HR_Employee_ResearchGuidanceArrayDTO { get; set; }
        public HR_Employee_BOSBOEDTO HR_Employee_BOSBOEDTO { get; set; }
        public HR_Employee_BOSBOEDTO[] HR_Employee_BOSBOEArrayDTO { get; set; }
        public HR_Employee_JournalDTO HR_Employee_JournalDTO { get; set; }
        public HR_Employee_JournalDTO[] HR_Employee_JournalArrayDTO { get; set; }
        public HR_Employee_ConferenceDTO HR_Employee_ConferenceDTO { get; set; }
        public HR_Employee_ConferenceDTO[] HR_Employee_ConferenceArrayDTO { get; set; }
        public HR_Employee_BookDTO HR_Employee_BookDTO { get; set; }
        public HR_Employee_BookDTO[] HR_Employee_BookArrayDTO { get; set; }
        public HR_Employee_BookChapterDTO HR_Employee_BookChapterDTO { get; set; }
        public HR_Employee_BookChapterDTO[] HR_Employee_BookChapterArrayDTO { get; set; }
        public HR_Employee_CommitteeDTO HR_Employee_CommitteeDTO { get; set; }
        public HR_Employee_CommitteeDTO[] HR_Employee_CommitteeArrayDTO { get; set; }
        public HR_Employee_OtherDetailsDTO HR_Employee_OtherDetailsDTO { get; set; }
        public HR_Employee_OtherDetailsDTO[] HR_Employee_OtherDetailsArrayDTO { get; set; }
        public HR_Employee_DevActivitiesDTO HR_Employee_DevActivitiesDTO { get; set; }
        public HR_Employee_DevActivitiesDTO[] HR_Employee_DevActivitiesArrayDTO { get; set; }
        public NAACPersonalDeatilsDTO[] NAACPersonalDeatilsDTO { get; set; }
        public HREmployeeGroupAExamDTO[] HREmployeeGroupAExamDTO { get; set; }
        public HREmployeeGroupAExamDTO[] HR_Employee_GroupADetailsArrayDTO { get; set; }
        public HREmployeeGroupBExamDTO[] HREmployeeGroupBExamDTO { get; set; }
        public HREmployeeGroupBExamDTO[] HR_Employee_GroupBDetailsArrayDTO { get; set; }
        public HR_Employee_ExamDutyDTO[] HREmployeeExamDutyDTO { get; set; }
        public HR_Employee_ExamDutyDTO[] HR_Employee_ExamDutyDetailsArrayDTO { get; set; }
        public HR_Employee_BOSBOE_FilesDTO[] HR_Employee_BOSBOE_FilesArrayDTO { get; set; }
    }

    public class NAACPersonalDeatilsDTO
    {
        public long HRME_Id { get; set; }
        public long? HRMD_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public DateTime? HRME_DOJ { get; set; }
        public string HRME_QualificationName { get; set; }
    }

}
