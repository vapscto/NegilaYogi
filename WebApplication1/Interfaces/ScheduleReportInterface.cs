using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
   public interface ScheduleReportInterface
    {
         ScheduleReportDTO getdetails(ScheduleReportDTO data);
        ScheduleReportDTO schedulelist(ScheduleReportDTO data);
       Task<ScheduleReportDTO> Getreportdetails(ScheduleReportDTO data);
       Task<ScheduleReportDTO> scheduleGetreportdetails(ScheduleReportDTO data);
       Task<ScheduleReportDTO> remarksGetreportdetails(ScheduleReportDTO data);

        Task<ScheduleReportDTO> sendmail (ScheduleReportDTO data);

    }
}
