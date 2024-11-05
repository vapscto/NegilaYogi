
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface ExamSubjectMappingInterface
    {
        ExamSubjectMappingDTO savedetails(ExamSubjectMappingDTO data);
        ExamSubjectMappingDTO get_category(ExamSubjectMappingDTO data);
        ExamSubjectMappingDTO get_subjects(ExamSubjectMappingDTO data);
        ExamSubjectMappingDTO deactivate(ExamSubjectMappingDTO data);
        ExamSubjectMappingDTO deactivate_sub(ExamSubjectMappingDTO data);
        ExamSubjectMappingDTO deactive_sub_exm(ExamSubjectMappingDTO data);
        ExamSubjectMappingDTO deactive_sub_subj(ExamSubjectMappingDTO data);
        ExamSubjectMappingDTO getalldetailsviewrecords(int id);
        ExamSubjectMappingDTO getalldetailsviewrecords_subexms(int id);
        ExamSubjectMappingDTO getalldetailsviewrecords_subsubjs(int id);
        ExamSubjectMappingDTO getalldetailsviewrecords_subsubjssunexam(int id);
        ExamSubjectMappingDTO editdetails(int id);
        ExamSubjectMappingDTO Getdetails(ExamSubjectMappingDTO data);
        ExamSubjectMappingDTO deactive_sub_subj_subexam(ExamSubjectMappingDTO data);
        ExamSubjectMappingDTO SetSubjectOrder(ExamSubjectMappingDTO data);
        ExamSubjectMappingDTO SaveSubjectOrder(ExamSubjectMappingDTO data);
        ExamSubjectMappingDTO deactive_subj_GradeList(ExamSubjectMappingDTO data);
        
    }
}
