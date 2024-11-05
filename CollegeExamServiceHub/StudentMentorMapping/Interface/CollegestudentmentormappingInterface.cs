using PreadmissionDTOs.com.vaps.College.Exam.StudentMentorMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.StudentMentorMapping.Interface
{
    public interface CollegestudentmentormappingInterface
    {
        CollegestudentmentormappingDTO Getdetails(CollegestudentmentormappingDTO data);
        CollegestudentmentormappingDTO onchangeyear(CollegestudentmentormappingDTO data);
        CollegestudentmentormappingDTO getbranch(CollegestudentmentormappingDTO data);
        CollegestudentmentormappingDTO getsemester(CollegestudentmentormappingDTO data);
        CollegestudentmentormappingDTO getsection(CollegestudentmentormappingDTO data);
        CollegestudentmentormappingDTO getemployee(CollegestudentmentormappingDTO data);
        CollegestudentmentormappingDTO getstudentdata(CollegestudentmentormappingDTO data);
        CollegestudentmentormappingDTO savedata(CollegestudentmentormappingDTO data);
        CollegestudentmentormappingDTO viewrecordspopup(CollegestudentmentormappingDTO data);
        CollegestudentmentormappingDTO Deletedata(CollegestudentmentormappingDTO data);

        //Report
        CollegestudentmentormappingDTO Getreportdetails(CollegestudentmentormappingDTO data);
        CollegestudentmentormappingDTO getreport(CollegestudentmentormappingDTO data);
    }
}
