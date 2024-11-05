using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface CollegeStudyCertificateReportInterface
    {
        CollegeStudyCertificateReportDTO getdata(CollegeStudyCertificateReportDTO data);
        CollegeStudyCertificateReportDTO onchangeyear(CollegeStudyCertificateReportDTO data);
        CollegeStudyCertificateReportDTO onchangecourse(CollegeStudyCertificateReportDTO data);
        CollegeStudyCertificateReportDTO onchangebranch(CollegeStudyCertificateReportDTO data);
        CollegeStudyCertificateReportDTO onchangesemester(CollegeStudyCertificateReportDTO data);
        CollegeStudyCertificateReportDTO searchfilter(CollegeStudyCertificateReportDTO data);
        CollegeStudyCertificateReportDTO onclickreport(CollegeStudyCertificateReportDTO data);
    }
}
