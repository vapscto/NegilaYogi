using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.HRMS;

namespace IVRMUX.Delegates.HRMS
{
    public class naacHrmsDetailsDelegate
    {
        CommonDelegate<HRMS_NAAC_DTO, HRMS_NAAC_DTO> _comm = new CommonDelegate<HRMS_NAAC_DTO, HRMS_NAAC_DTO>();
        CommonDelegate<HR_Employee_OrientationCourseDTO, HR_Employee_OrientationCourseDTO> _commOrient = new CommonDelegate<HR_Employee_OrientationCourseDTO, HR_Employee_OrientationCourseDTO>();
        CommonDelegate<HR_Employee_StudentActivitiesDTO, HR_Employee_StudentActivitiesDTO> _commStActi = new CommonDelegate<HR_Employee_StudentActivitiesDTO, HR_Employee_StudentActivitiesDTO>();
        CommonDelegate<HR_Employee_DevActivitiesDTO, HR_Employee_DevActivitiesDTO> _commDVActi = new CommonDelegate<HR_Employee_DevActivitiesDTO, HR_Employee_DevActivitiesDTO>();
        CommonDelegate<HR_Employee_ResearchProjectsDTO, HR_Employee_ResearchProjectsDTO> _commResPrj = new CommonDelegate<HR_Employee_ResearchProjectsDTO, HR_Employee_ResearchProjectsDTO>();
        CommonDelegate<HR_Employee_ResearchGuidanceDTO, HR_Employee_ResearchGuidanceDTO> _commResGuid = new CommonDelegate<HR_Employee_ResearchGuidanceDTO, HR_Employee_ResearchGuidanceDTO>();
        CommonDelegate<HR_Employee_BOSBOEDTO, HR_Employee_BOSBOEDTO> _commBOSBOE = new CommonDelegate<HR_Employee_BOSBOEDTO, HR_Employee_BOSBOEDTO>();
        CommonDelegate<HR_Employee_JournalDTO, HR_Employee_JournalDTO> _commJor = new CommonDelegate<HR_Employee_JournalDTO, HR_Employee_JournalDTO>();
        CommonDelegate<HR_Employee_ConferenceDTO, HR_Employee_ConferenceDTO> _commConfe = new CommonDelegate<HR_Employee_ConferenceDTO, HR_Employee_ConferenceDTO>();
        CommonDelegate<HR_Employee_BookDTO, HR_Employee_BookDTO> _commBook = new CommonDelegate<HR_Employee_BookDTO, HR_Employee_BookDTO>();
        CommonDelegate<HR_Employee_BookChapterDTO, HR_Employee_BookChapterDTO> _commBokChap = new CommonDelegate<HR_Employee_BookChapterDTO, HR_Employee_BookChapterDTO>();
        CommonDelegate<HR_Employee_CommitteeDTO, HR_Employee_CommitteeDTO> _commComme = new CommonDelegate<HR_Employee_CommitteeDTO, HR_Employee_CommitteeDTO>();
        CommonDelegate<HR_Employee_OtherDetailsDTO, HR_Employee_OtherDetailsDTO> _commOthDet = new CommonDelegate<HR_Employee_OtherDetailsDTO, HR_Employee_OtherDetailsDTO>();
        CommonDelegate<HR_Employee_ExamDutyDTO, HR_Employee_ExamDutyDTO> _commExamDet = new CommonDelegate<HR_Employee_ExamDutyDTO, HR_Employee_ExamDutyDTO>();

        public HRMS_NAAC_DTO getdetails(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsFacade/getdetails");
        }

        public HRMS_NAAC_DTO get_depts(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsFacade/get_depts");
        }

        public HRMS_NAAC_DTO get_desig(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsFacade/get_desig");
        }
        
        public HRMS_NAAC_DTO get_Employe_ob(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsFacade/get_Employe_ob");
        }

        public HRMS_NAAC_DTO SaveData(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsFacade/SaveData");
        }

        public HRMS_NAAC_DTO getOrientdata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsFacade/getOrientdata");
        }

        public HRMS_NAAC_DTO getStudentActivitydata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsFacade/getStudentActivitydata");
        }

        public HRMS_NAAC_DTO getProfessionalActivitydata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsFacade/getProfessionalActivitydata");
        }

        public HRMS_NAAC_DTO getResearchProjectdata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsFacade/getResearchProjectdata");
        }

        public HRMS_NAAC_DTO getResearchGuidedata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsFacade/getResearchGuidedata");
        }

        public HRMS_NAAC_DTO getBOSBOEdata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsFacade/getBOSBOEdata");
        }

        public HRMS_NAAC_DTO getJournaldata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsFacade/getJournaldata");
        }

        public HRMS_NAAC_DTO getConferencedata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsFacade/getConferencedata");
        }

        public HRMS_NAAC_DTO getBookdata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsFacade/getBookdata");
        }

        public HRMS_NAAC_DTO getBookChapterdata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsFacade/getBookChapterdata");
        }

        public HRMS_NAAC_DTO getCommetteedata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsFacade/getCommetteedata");
        }

        public HRMS_NAAC_DTO getOtherDetaildata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsFacade/getOtherDetaildata");
        }

        public HRMS_NAAC_DTO get_EmployeALLDATA(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsFacade/get_EmployeALLDATA");
        }

        public HR_Employee_OrientationCourseDTO DeleteDocumentRecordOrientation(HR_Employee_OrientationCourseDTO data)
        {
            return _commOrient.naacdetailsbypost(data, "naacHrmsDetailsFacade/DeleteDocumentRecordOrientation");
        }

        public HR_Employee_StudentActivitiesDTO DeleteDocumentRecordStuActivity(HR_Employee_StudentActivitiesDTO data)
        {
            return _commStActi.naacdetailsbypost(data, "naacHrmsDetailsFacade/DeleteDocumentRecordStuActivity");
        }

        public HR_Employee_DevActivitiesDTO DeleteDocumentRecordProfActivity(HR_Employee_DevActivitiesDTO data)
        {
            return _commDVActi.naacdetailsbypost(data, "naacHrmsDetailsFacade/DeleteDocumentRecordProfActivity");
        }

        public HR_Employee_ResearchProjectsDTO DeleteDocumentRecordResProj(HR_Employee_ResearchProjectsDTO data)
        {
            return _commResPrj.naacdetailsbypost(data, "naacHrmsDetailsFacade/DeleteDocumentRecordResProj");
        }

        public HR_Employee_ResearchGuidanceDTO DeleteDocumentRecordResGuide(HR_Employee_ResearchGuidanceDTO data)
        {
            return _commResGuid.naacdetailsbypost(data, "naacHrmsDetailsFacade/DeleteDocumentRecordResGuide");
        }

        public HR_Employee_BOSBOEDTO DeleteDocumentRecordBOSBOE(HR_Employee_BOSBOEDTO data)
        {
            return _commBOSBOE.naacdetailsbypost(data, "naacHrmsDetailsFacade/DeleteDocumentRecordBOSBOE");
        }

        public HR_Employee_JournalDTO DeleteDocumentRecordJournal(HR_Employee_JournalDTO data)
        {
            return _commJor.naacdetailsbypost(data, "naacHrmsDetailsFacade/DeleteDocumentRecordJournal");
        }

        public HR_Employee_ConferenceDTO DeleteDocumentRecordConference(HR_Employee_ConferenceDTO data)
        {
            return _commConfe.naacdetailsbypost(data, "naacHrmsDetailsFacade/DeleteDocumentRecordConference");
        }

        public HR_Employee_BookDTO DeleteDocumentRecordBook(HR_Employee_BookDTO data)
        {
            return _commBook.naacdetailsbypost(data, "naacHrmsDetailsFacade/DeleteDocumentRecordBook");
        }

        public HR_Employee_BookChapterDTO DeleteDocumentRecordBookChapter(HR_Employee_BookChapterDTO data)
        {
            return _commBokChap.naacdetailsbypost(data, "naacHrmsDetailsFacade/DeleteDocumentRecordBookChapter");
        }

        public HR_Employee_CommitteeDTO DeleteDocumentRecordCommettee(HR_Employee_CommitteeDTO data)
        {
            return _commComme.naacdetailsbypost(data, "naacHrmsDetailsFacade/DeleteDocumentRecordCommettee");
        }

        public HR_Employee_OtherDetailsDTO DeleteDocumentRecordOthers(HR_Employee_OtherDetailsDTO data)
        {
            return _commOthDet.naacdetailsbypost(data, "naacHrmsDetailsFacade/DeleteDocumentRecordOthers");
        }

        public HR_Employee_ExamDutyDTO DeleteDocumentRecordExamination(HR_Employee_ExamDutyDTO data)
        {
            return _commExamDet.naacdetailsbypost(data, "naacHrmsDetailsFacade/DeleteDocumentRecordExamination");
        }

        public HRMS_NAAC_DTO editRecord(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsFacade/editRecord");
        }
    }
}
