using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Interfaces
{
 public   interface ClgExamStandardInterface
    {
        ExamStandardDTO savedetails(ExamStandardDTO data);
        ExamStandardDTO Getdetails(ExamStandardDTO id);
    }
}
