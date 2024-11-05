using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Interfaces
{
  public interface ClgCumulativeReportInterface
    {
        ClgCumulativeReportDTO Getdetails(ClgCumulativeReportDTO data);
        Task<ClgCumulativeReportDTO> Getcmreport(ClgCumulativeReportDTO data);
        ClgCumulativeReportDTO onchangeyear(ClgCumulativeReportDTO data);
        ClgCumulativeReportDTO onchangecourse(ClgCumulativeReportDTO data);
        ClgCumulativeReportDTO onchangebranch(ClgCumulativeReportDTO data);
        ClgCumulativeReportDTO onchangesemester(ClgCumulativeReportDTO data);
        ClgCumulativeReportDTO onchangesubjectscheme(ClgCumulativeReportDTO data);
        ClgCumulativeReportDTO onchangeschemetype(ClgCumulativeReportDTO data);
        ClgCumulativeReportDTO GetCumulativeReportFormat2(ClgCumulativeReportDTO data);
        ClgCumulativeReportDTO GetProgresscardReport(ClgCumulativeReportDTO data);
        ClgCumulativeReportDTO GetJNUProgressCardReport1(ClgCumulativeReportDTO data);
    }
}