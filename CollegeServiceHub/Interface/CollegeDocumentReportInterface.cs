using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface CollegeDocumentReportInterface
    {
        CollegeDocumentReportDTO getdetails(CollegeDocumentReportDTO data);
        CollegeDocumentReportDTO onchangeyear(CollegeDocumentReportDTO data);
        CollegeDocumentReportDTO onchangecourse(CollegeDocumentReportDTO data);
        CollegeDocumentReportDTO onchangebranch(CollegeDocumentReportDTO data);
        CollegeDocumentReportDTO onchangesemester(CollegeDocumentReportDTO data);
        CollegeDocumentReportDTO onchangesection(CollegeDocumentReportDTO data);
        CollegeDocumentReportDTO getreportdetails(CollegeDocumentReportDTO data);

        CollegeDocumentReportDTO getdetails_view(CollegeDocumentReportDTO id);
        CollegeDocumentReportDTO getclgstudata_view(CollegeDocumentReportDTO id);
    }
}
