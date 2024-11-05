using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.com.vaps.LessonPlanner.Interface
{
    public interface SchoolStaffperiodtransactionreportInterface
    {
        SchoolStaffperiodtransactionreportDTO Getdetailstransaction(SchoolStaffperiodtransactionreportDTO data);
        SchoolStaffperiodtransactionreportDTO onselectAcdYear(SchoolStaffperiodtransactionreportDTO data);
        SchoolStaffperiodtransactionreportDTO onselectclass(SchoolStaffperiodtransactionreportDTO data);     
        SchoolStaffperiodtransactionreportDTO onchangesection(SchoolStaffperiodtransactionreportDTO data);
        SchoolStaffperiodtransactionreportDTO getreport(SchoolStaffperiodtransactionreportDTO data);
        SchoolStaffperiodtransactionreportDTO getdevationreport(SchoolStaffperiodtransactionreportDTO data);
    }
}
