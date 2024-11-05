using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface VikasaHallTicketReportInterface
    {
        VikasaHallTicketReportDTO getdetails(VikasaHallTicketReportDTO data);
        VikasaHallTicketReportDTO onselectAcdYear(VikasaHallTicketReportDTO data);
        VikasaHallTicketReportDTO onselectclass(VikasaHallTicketReportDTO data);
        VikasaHallTicketReportDTO onselectSection(VikasaHallTicketReportDTO data);
        VikasaHallTicketReportDTO report(VikasaHallTicketReportDTO data);

    }
}
