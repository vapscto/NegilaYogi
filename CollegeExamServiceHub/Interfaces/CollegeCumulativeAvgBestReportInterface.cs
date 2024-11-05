using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.College.Exam;
namespace CollegeExamServiceHub.Interfaces
{
    public interface CollegeCumulativeAvgBestReportInterface
    {
        CollegeCumulativeAvgBestReportDTO Getdetails(CollegeCumulativeAvgBestReportDTO data);
        CollegeCumulativeAvgBestReportDTO onchangeyear(CollegeCumulativeAvgBestReportDTO data);
        CollegeCumulativeAvgBestReportDTO onchangecourse(CollegeCumulativeAvgBestReportDTO data);
        CollegeCumulativeAvgBestReportDTO onchangebranch(CollegeCumulativeAvgBestReportDTO data);
        CollegeCumulativeAvgBestReportDTO onchangesemester(CollegeCumulativeAvgBestReportDTO data);
        CollegeCumulativeAvgBestReportDTO onchangesubjectscheme(CollegeCumulativeAvgBestReportDTO data);
        CollegeCumulativeAvgBestReportDTO onchangeschemetype(CollegeCumulativeAvgBestReportDTO data);
        CollegeCumulativeAvgBestReportDTO Getcmreport(CollegeCumulativeAvgBestReportDTO data);
        CollegeCumulativeAvgBestReportDTO getindreport(CollegeCumulativeAvgBestReportDTO data);
    }
}
