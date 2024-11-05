using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface ExamTermAndExamMappingInterface
    {

        ExamTermAndExamMappingDTO deactivate(ExamTermAndExamMappingDTO data);
        ExamTermAndExamMappingDTO savedetail(ExamTermAndExamMappingDTO data);
        ExamTermAndExamMappingDTO savetermmap(ExamTermAndExamMappingDTO data);
        ExamTermAndExamMappingDTO editdetails(int ID);
        ExamTermAndExamMappingDTO deleterec(int id);
        ExamTermAndExamMappingDTO edittermmap(int id);
        ExamTermAndExamMappingDTO getexampopup(int id);
        ExamTermAndExamMappingDTO deactivate1(ExamTermAndExamMappingDTO data);
        ExamTermAndExamMappingDTO deactive_sub(ExamTermAndExamMappingDTO data);
        ExamTermAndExamMappingDTO Getdetails(ExamTermAndExamMappingDTO data);
        ExamTermAndExamMappingDTO ontermchange(ExamTermAndExamMappingDTO data);
        ExamTermAndExamMappingDTO get_exam(ExamTermAndExamMappingDTO data);

        // New Coding
        ExamTermAndExamMappingDTO onchangeyear(ExamTermAndExamMappingDTO data);
        ExamTermAndExamMappingDTO onchangecategory(ExamTermAndExamMappingDTO data);
        ExamTermAndExamMappingDTO checktermname(ExamTermAndExamMappingDTO data);
        ExamTermAndExamMappingDTO saveddata(ExamTermAndExamMappingDTO data);
        ExamTermAndExamMappingDTO editdetailsnew(ExamTermAndExamMappingDTO data);
        ExamTermAndExamMappingDTO viewrecordspopup(ExamTermAndExamMappingDTO data);
        ExamTermAndExamMappingDTO deactivatenew(ExamTermAndExamMappingDTO data);
        ExamTermAndExamMappingDTO deactivesub(ExamTermAndExamMappingDTO data);
        
    }
}
