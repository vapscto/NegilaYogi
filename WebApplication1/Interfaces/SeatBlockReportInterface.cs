using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface SeatBlockReportInterface
    {
        SeatBlockReportDTO getdetails(SeatBlockReportDTO data);
        SeatBlockReportDTO Getstudlist(SeatBlockReportDTO data);
       // SeatBlockReportDTO getstuddetails(SeatBlockReportDTO data);
        Task<SeatBlockReportDTO> Getreportdetails(SeatBlockReportDTO data);
        
    }
}
