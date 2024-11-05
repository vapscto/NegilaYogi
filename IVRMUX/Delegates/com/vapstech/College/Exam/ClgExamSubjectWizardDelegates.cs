using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Exam
{
    public class ClgExamSubjectWizardDelegates
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgSubjectWizardDTO, ClgSubjectWizardDTO> COMMM = new CommonDelegate<ClgSubjectWizardDTO, ClgSubjectWizardDTO>();

        public ClgSubjectWizardDTO Getdetails(ClgSubjectWizardDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExamSubjectWizardFacade/Getdetails/");
        }
        public ClgSubjectWizardDTO getbranch(ClgSubjectWizardDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExamSubjectWizardFacade/getbranch/");
        }
        public ClgSubjectWizardDTO getsemester(ClgSubjectWizardDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExamSubjectWizardFacade/getsemester/");
        }
        public ClgSubjectWizardDTO getsubjectscheme(ClgSubjectWizardDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExamSubjectWizardFacade/getsubjectscheme/");
        }
        public ClgSubjectWizardDTO getsubjectschemetype(ClgSubjectWizardDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExamSubjectWizardFacade/getsubjectschemetype/");
        }
        public ClgSubjectWizardDTO getsubjectgroup(ClgSubjectWizardDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExamSubjectWizardFacade/getsubjectgroup/");
        }

        public ClgSubjectWizardDTO savedetails(ClgSubjectWizardDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExamSubjectWizardFacade/savedetails/");
        }
        public ClgSubjectWizardDTO getalldetailsviewrecords(int data)
        {
            return COMMM.GETexam(data, "ClgExamSubjectWizardFacade/getalldetailsviewrecords/");
        }
        public ClgSubjectWizardDTO getalldetailsviewrecords_subexms(int data)
        {
            return COMMM.GETexam(data, "ClgExamSubjectWizardFacade/getalldetailsviewrecords_subexms/");
        }
        public ClgSubjectWizardDTO getalldetailsviewrecords_subsubjs(int data)
        {
            return COMMM.GETexam(data, "ClgExamSubjectWizardFacade/getalldetailsviewrecords_subsubjs/");
        }
        public ClgSubjectWizardDTO deactivate_sub(ClgSubjectWizardDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExamSubjectWizardFacade/deactivate_sub/");
        }
        public ClgSubjectWizardDTO deactive_sub_exm(ClgSubjectWizardDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExamSubjectWizardFacade/deactive_sub_exm/");
        }
        public ClgSubjectWizardDTO deactive_sub_subj(ClgSubjectWizardDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExamSubjectWizardFacade/deactive_sub_subj/");
        }
        public ClgSubjectWizardDTO editdetails(int data)
        {
            return COMMM.GETexam(data, "ClgExamSubjectWizardFacade/editdetails/");
        }
        public ClgSubjectWizardDTO deactivate(ClgSubjectWizardDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExamSubjectWizardFacade/deactivate/");
        }
        public ClgSubjectWizardDTO getalldetailsviewrecords_subsubjssunexam(ClgSubjectWizardDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExamSubjectWizardFacade/getalldetailsviewrecords_subsubjssunexam/");
        }
        public ClgSubjectWizardDTO deactive_sub_subj_subexam(ClgSubjectWizardDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExamSubjectWizardFacade/deactive_sub_subj_subexam/");
        }
        public ClgSubjectWizardDTO get_subjects(ClgSubjectWizardDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExamSubjectWizardFacade/get_subjects/");
        }
        public ClgSubjectWizardDTO SetOrder_SubSubject(ClgSubjectWizardDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExamSubjectWizardFacade/SetOrder_SubSubject/");
        }
        
    }
}
