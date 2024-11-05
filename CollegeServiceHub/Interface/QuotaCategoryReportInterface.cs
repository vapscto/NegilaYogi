using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface QuotaCategoryReportInterface
    {
        QuotaCategoryReportDTO getdetails(QuotaCategoryReportDTO data);
        QuotaCategoryReportDTO onselectAcdYear(QuotaCategoryReportDTO data);
        QuotaCategoryReportDTO onselectCourse(QuotaCategoryReportDTO data);
        QuotaCategoryReportDTO onselectBranch(QuotaCategoryReportDTO data);
        
        QuotaCategoryReportDTO onreport(QuotaCategoryReportDTO data);
    }
}
