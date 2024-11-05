using PreadmissionDTOs.FeedBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.FeedBack.Interface
{
    public interface AcademicCalenderReportInterface
    {

        AcademicCalenderReportDTO getdetails(AcademicCalenderReportDTO data);
        AcademicCalenderReportDTO getreport(AcademicCalenderReportDTO data);
    }
}
