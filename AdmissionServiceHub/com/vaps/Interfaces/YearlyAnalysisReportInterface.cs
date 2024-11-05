using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface YearlyAnalysisReportInterface
    {
        YearlyAnalysisReportDTO loaddata(YearlyAnalysisReportDTO data);
        YearlyAnalysisReportDTO report(YearlyAnalysisReportDTO data);
    }
}
