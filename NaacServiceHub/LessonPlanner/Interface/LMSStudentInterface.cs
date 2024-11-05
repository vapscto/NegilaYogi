using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.NAAC.LessonPlanner;

namespace NaacServiceHub.LessonPlanner.Interface
{
    public interface LMSStudentInterface
    {
        LMSStudentDTO Getdetails(LMSStudentDTO data);
        LMSStudentDTO onchangesemester(LMSStudentDTO data);
        LMSStudentDTO getcollegetopics(LMSStudentDTO data);
        LMSStudentDTO getcollegedocuments(LMSStudentDTO data);
 
        // School 
        LMSStudentDTO Getdetailsschool(LMSStudentDTO data);
        LMSStudentDTO onchangeyear(LMSStudentDTO data);
        LMSStudentDTO onchangeclass(LMSStudentDTO data);
        LMSStudentDTO getschooltopics(LMSStudentDTO data);
        LMSStudentDTO getschooldocuments(LMSStudentDTO data);
    }
}
