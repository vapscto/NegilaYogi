using PreadmissionDTOs.com.vaps.College.Exam.LessonPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.LessonPlanner.Interface
{
    public interface CollegeStaffPeriodMappingInterface
    {
        CollegeStaffPeriodMappingDTO Getdetails(CollegeStaffPeriodMappingDTO data);
        CollegeStaffPeriodMappingDTO getemployeedetails(CollegeStaffPeriodMappingDTO data);
        CollegeStaffPeriodMappingDTO onchangecourse(CollegeStaffPeriodMappingDTO data);
        CollegeStaffPeriodMappingDTO onchangebranch(CollegeStaffPeriodMappingDTO data);
        CollegeStaffPeriodMappingDTO onchangesemster(CollegeStaffPeriodMappingDTO data);
        CollegeStaffPeriodMappingDTO onchangesection(CollegeStaffPeriodMappingDTO data);
        CollegeStaffPeriodMappingDTO getsearchdetails(CollegeStaffPeriodMappingDTO data);
        CollegeStaffPeriodMappingDTO savedata(CollegeStaffPeriodMappingDTO data);

        // Staff Transaction
        CollegeStaffPeriodMappingDTO Getdetailstransaction(CollegeStaffPeriodMappingDTO data);
        CollegeStaffPeriodMappingDTO getsearchdetailstransaction(CollegeStaffPeriodMappingDTO data);
        CollegeStaffPeriodMappingDTO savedatatransaction(CollegeStaffPeriodMappingDTO data);
        
    }
}
