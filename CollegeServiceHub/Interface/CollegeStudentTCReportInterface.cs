using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface CollegeStudentTCReportInterface
    {
        CollegeStudentTCReportDTO getalldetails(CollegeStudentTCReportDTO data);
        CollegeStudentTCReportDTO onchangeyear(CollegeStudentTCReportDTO data);
        CollegeStudentTCReportDTO onchangecourse(CollegeStudentTCReportDTO data);
        CollegeStudentTCReportDTO onchangebranch(CollegeStudentTCReportDTO data);
        CollegeStudentTCReportDTO onchangesemester(CollegeStudentTCReportDTO data);
        CollegeStudentTCReportDTO Getreportdetails(CollegeStudentTCReportDTO data);

        // TC Custom Report
        CollegeStudentTCReportDTO onchangeyeartc(CollegeStudentTCReportDTO data);
        CollegeStudentTCReportDTO stdnamechange(CollegeStudentTCReportDTO data);
        CollegeStudentTCReportDTO getTcdetails(CollegeStudentTCReportDTO data);
        
    }
}
