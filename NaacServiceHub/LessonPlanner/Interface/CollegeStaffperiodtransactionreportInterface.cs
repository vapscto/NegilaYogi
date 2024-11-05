using PreadmissionDTOs.com.vaps.College.Exam.LessonPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.LessonPlanner.Interface
{
    public interface CollegeStaffperiodtransactionreportInterface
    {
        CollegeStaffperiodtransactionreportDTO Getdetailstransaction(CollegeStaffperiodtransactionreportDTO data);
        CollegeStaffperiodtransactionreportDTO onselectAcdYear(CollegeStaffperiodtransactionreportDTO data);
        CollegeStaffperiodtransactionreportDTO onselectCourse(CollegeStaffperiodtransactionreportDTO data);
        CollegeStaffperiodtransactionreportDTO onselectBranch(CollegeStaffperiodtransactionreportDTO data);
        CollegeStaffperiodtransactionreportDTO getsection(CollegeStaffperiodtransactionreportDTO data);
        CollegeStaffperiodtransactionreportDTO onchangesection(CollegeStaffperiodtransactionreportDTO data);
        CollegeStaffperiodtransactionreportDTO getreport(CollegeStaffperiodtransactionreportDTO data);
        CollegeStaffperiodtransactionreportDTO getdevationreport(CollegeStaffperiodtransactionreportDTO data);
        
    }
}
