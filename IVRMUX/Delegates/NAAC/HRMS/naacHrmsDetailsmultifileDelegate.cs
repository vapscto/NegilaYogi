using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.HRMS;

namespace IVRMUX.Delegates.HRMS
{
    public class naacHrmsDetailsmultifileDelegate
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
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/getdetails");
        }

        public HRMS_NAAC_DTO get_depts(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/get_depts");
        }

        public HRMS_NAAC_DTO get_desig(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/get_desig");
        }
        
        public HRMS_NAAC_DTO get_Employe_ob(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/get_Employe_ob");
        }

        public HRMS_NAAC_DTO viewfileremark(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/viewfileremark");
        }

        public HRMS_NAAC_DTO viewsubfileremark(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/viewsubfileremark");
        }

        public HRMS_NAAC_DTO SaveData(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/SaveData");
        }

        public HRMS_NAAC_DTO getOrientdata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/getOrientdata");
        }

        public HRMS_NAAC_DTO getStudentActivitydata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/getStudentActivitydata");
        }

        public HRMS_NAAC_DTO getProfessionalActivitydata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/getProfessionalActivitydata");
        }

        public HRMS_NAAC_DTO getResearchProjectdata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/getResearchProjectdata");
        }

        public HRMS_NAAC_DTO getResearchGuidedata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/getResearchGuidedata");
        }

        public HRMS_NAAC_DTO getBOSBOEdata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/getBOSBOEdata");
        }

        public HRMS_NAAC_DTO getJournaldata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/getJournaldata");
        }

        public HRMS_NAAC_DTO getConferencedata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/getConferencedata");
        }

        public HRMS_NAAC_DTO getBookdata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/getBookdata");
        }

        public HRMS_NAAC_DTO getBookChapterdata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/getBookChapterdata");
        }

        public HRMS_NAAC_DTO getCommetteedata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/getCommetteedata");
        }

        public HRMS_NAAC_DTO getOtherDetaildata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/getOtherDetaildata");
        }

        public HRMS_NAAC_DTO get_EmployeALLDATA(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/get_EmployeALLDATA");
        }

        public HR_Employee_OrientationCourseDTO DeleteDocumentRecordOrientation(HR_Employee_OrientationCourseDTO data)
        {
            return _commOrient.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/DeleteDocumentRecordOrientation");
        }

        public HR_Employee_StudentActivitiesDTO DeleteDocumentRecordStuActivity(HR_Employee_StudentActivitiesDTO data)
        {
            return _commStActi.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/DeleteDocumentRecordStuActivity");
        }

        public HR_Employee_DevActivitiesDTO DeleteDocumentRecordProfActivity(HR_Employee_DevActivitiesDTO data)
        {
            return _commDVActi.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/DeleteDocumentRecordProfActivity");
        }

        public HR_Employee_ResearchProjectsDTO DeleteDocumentRecordResProj(HR_Employee_ResearchProjectsDTO data)
        {
            return _commResPrj.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/DeleteDocumentRecordResProj");
        }

        public HR_Employee_ResearchGuidanceDTO DeleteDocumentRecordResGuide(HR_Employee_ResearchGuidanceDTO data)
        {
            return _commResGuid.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/DeleteDocumentRecordResGuide");
        }

        public HR_Employee_BOSBOEDTO DeleteDocumentRecordBOSBOE(HR_Employee_BOSBOEDTO data)
        {
            return _commBOSBOE.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/DeleteDocumentRecordBOSBOE");
        }

        public HR_Employee_JournalDTO DeleteDocumentRecordJournal(HR_Employee_JournalDTO data)
        {
            return _commJor.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/DeleteDocumentRecordJournal");
        }

        public HR_Employee_ConferenceDTO DeleteDocumentRecordConference(HR_Employee_ConferenceDTO data)
        {
            return _commConfe.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/DeleteDocumentRecordConference");
        }

        public HR_Employee_BookDTO DeleteDocumentRecordBook(HR_Employee_BookDTO data)
        {
            return _commBook.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/DeleteDocumentRecordBook");
        }

        public HR_Employee_BookChapterDTO DeleteDocumentRecordBookChapter(HR_Employee_BookChapterDTO data)
        {
            return _commBokChap.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/DeleteDocumentRecordBookChapter");
        }

        public HR_Employee_CommitteeDTO DeleteDocumentRecordCommettee(HR_Employee_CommitteeDTO data)
        {
            return _commComme.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/DeleteDocumentRecordCommettee");
        }

        public HR_Employee_OtherDetailsDTO DeleteDocumentRecordOthers(HR_Employee_OtherDetailsDTO data)
        {
            return _commOthDet.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/DeleteDocumentRecordOthers");
        }

        public HR_Employee_ExamDutyDTO DeleteDocumentRecordExamination(HR_Employee_ExamDutyDTO data)
        {
            return _commExamDet.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/DeleteDocumentRecordExamination");
        }

        public HRMS_NAAC_DTO editRecord(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "naacHrmsDetailsmultifileFacade/editRecord");
        }
    }
}
