using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
   public interface StudentActiveInactiveReportInterface
    {
        StudentActiveInactiveReportDTO getdata(StudentActiveInactiveReportDTO data);
        StudentActiveInactiveReportDTO onacademicyearchange(StudentActiveInactiveReportDTO data);
        StudentActiveInactiveReportDTO oncoursechange(StudentActiveInactiveReportDTO data);
        StudentActiveInactiveReportDTO onbranchchange(StudentActiveInactiveReportDTO data);
        StudentActiveInactiveReportDTO onchangesemester(StudentActiveInactiveReportDTO data);
        StudentActiveInactiveReportDTO getreport(StudentActiveInactiveReportDTO data);
    }
}
