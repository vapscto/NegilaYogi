using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
   public interface MeritListReportInterface
    {
        SMSReportDTO getdetails(SMSReportDTO data);
        Task<SMSReportDTO> Getreportdetails(SMSReportDTO data);

    }
}
