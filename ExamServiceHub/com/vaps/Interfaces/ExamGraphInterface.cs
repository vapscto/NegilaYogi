using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface ExamGraphInterface
    {
        ExamGraphDTO getdetails(ExamGraphDTO data);
        ExamGraphDTO onreport(ExamGraphDTO data);
        ExamGraphDTO OnAcdyear(ExamGraphDTO data);
        ExamGraphDTO onclasschange(ExamGraphDTO data);
        ExamGraphDTO onsectionchange(ExamGraphDTO data);
        ExamGraphDTO onchangeexam(ExamGraphDTO data);
        ExamGraphDTO onchangecategory(ExamGraphDTO data);
        ExamGraphDTO onchangesubject(ExamGraphDTO data);
        
    }
}
