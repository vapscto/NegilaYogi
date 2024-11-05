using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Interfaces
{
  public  interface ClgExammarksCalculationInterface
    {
        ClgMarksCalculationsDTO Getdetails(ClgMarksCalculationsDTO data);
        ClgMarksCalculationsDTO Calculation(ClgMarksCalculationsDTO data);
    }
}
