using PreadmissionDTOs.com.vaps.College.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Interfaces
{
    public interface ClgExamSubjectWizardInterface
    {
        ClgSubjectWizardDTO Getdetails(ClgSubjectWizardDTO data);
        ClgSubjectWizardDTO getbranch(ClgSubjectWizardDTO data);
        ClgSubjectWizardDTO getsemester(ClgSubjectWizardDTO data);
        ClgSubjectWizardDTO getsubjectscheme(ClgSubjectWizardDTO data);
        ClgSubjectWizardDTO getsubjectschemetype(ClgSubjectWizardDTO data);
        ClgSubjectWizardDTO getsubjectgroup(ClgSubjectWizardDTO data);

        ClgSubjectWizardDTO savedetails(ClgSubjectWizardDTO data);
        ClgSubjectWizardDTO getalldetailsviewrecords(int id);
        ClgSubjectWizardDTO getalldetailsviewrecords_subexms(int id);
        ClgSubjectWizardDTO getalldetailsviewrecords_subsubjs(int id);
        ClgSubjectWizardDTO deactivate_sub(ClgSubjectWizardDTO id);
        ClgSubjectWizardDTO deactive_sub_exm(ClgSubjectWizardDTO id);
        ClgSubjectWizardDTO deactive_sub_subj(ClgSubjectWizardDTO id);
        ClgSubjectWizardDTO editdetails(int id);
        ClgSubjectWizardDTO deactivate(ClgSubjectWizardDTO id);
        ClgSubjectWizardDTO getalldetailsviewrecords_subsubjssunexam(ClgSubjectWizardDTO id);
        ClgSubjectWizardDTO deactive_sub_subj_subexam(ClgSubjectWizardDTO id);
        ClgSubjectWizardDTO get_subjects(ClgSubjectWizardDTO id);
        ClgSubjectWizardDTO SetOrder_SubSubject(ClgSubjectWizardDTO id);
    }
}