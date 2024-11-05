using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Interfaces
{
    public interface CollegeBMCPUProgresscardReportInterface
    {
        CollegeBMCPUProgresscardReportDTO Getdetails(CollegeBMCPUProgresscardReportDTO data);
        CollegeBMCPUProgresscardReportDTO OnAcdyear(CollegeBMCPUProgresscardReportDTO data);
        CollegeBMCPUProgresscardReportDTO onchangecourse(CollegeBMCPUProgresscardReportDTO data);
        CollegeBMCPUProgresscardReportDTO onchangebranch(CollegeBMCPUProgresscardReportDTO data);
        CollegeBMCPUProgresscardReportDTO onchangesemester(CollegeBMCPUProgresscardReportDTO data);
        CollegeBMCPUProgresscardReportDTO onchangesection(CollegeBMCPUProgresscardReportDTO data);
        CollegeBMCPUProgresscardReportDTO onchangesubjectscheme(CollegeBMCPUProgresscardReportDTO data);
        CollegeBMCPUProgresscardReportDTO onchangeschemetype(CollegeBMCPUProgresscardReportDTO data);
        CollegeBMCPUProgresscardReportDTO getreport(CollegeBMCPUProgresscardReportDTO data);
    }
}
