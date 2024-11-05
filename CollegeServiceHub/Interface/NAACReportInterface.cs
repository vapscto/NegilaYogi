using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface NAACReportInterface
    {
        NAACReportDTO getdetails(NAACReportDTO data);
        NAACReportDTO onreport(NAACReportDTO data);
    }
}
