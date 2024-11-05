using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface VikasaAssessment2ReportInterface
    {
        VikasaSubjectwiseCumulativeReportDTO showdetails(VikasaSubjectwiseCumulativeReportDTO data);        
        VikasaSubjectwiseCumulativeReportDTO Getdetails(VikasaSubjectwiseCumulativeReportDTO data);
        VikasaSubjectwiseCumulativeReportDTO get_class(VikasaSubjectwiseCumulativeReportDTO data);
        VikasaSubjectwiseCumulativeReportDTO get_section(VikasaSubjectwiseCumulativeReportDTO data);
        VikasaSubjectwiseCumulativeReportDTO get_exam(VikasaSubjectwiseCumulativeReportDTO data);
        VikasaSubjectwiseCumulativeReportDTO getcategory(VikasaSubjectwiseCumulativeReportDTO data);
    }
}