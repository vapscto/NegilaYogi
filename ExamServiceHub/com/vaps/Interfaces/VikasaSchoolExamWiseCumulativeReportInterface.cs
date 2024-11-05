using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface VikasaSchoolExamWiseCumulativeReportInterface
    {
        VikasaSubjectwiseCumulativeReportDTO showdetails(VikasaSubjectwiseCumulativeReportDTO data);
        VikasaSubjectwiseCumulativeReportDTO Getdetails(VikasaSubjectwiseCumulativeReportDTO data);
        VikasaSubjectwiseCumulativeReportDTO get_class(VikasaSubjectwiseCumulativeReportDTO data);
        VikasaSubjectwiseCumulativeReportDTO get_section(VikasaSubjectwiseCumulativeReportDTO data);
        VikasaSubjectwiseCumulativeReportDTO get_subject(VikasaSubjectwiseCumulativeReportDTO data);
        VikasaSubjectwiseCumulativeReportDTO get_Exam(VikasaSubjectwiseCumulativeReportDTO data);
        Task<VikasaSubjectwiseCumulativeReportDTO> savedetails(VikasaSubjectwiseCumulativeReportDTO data);
    }
}
