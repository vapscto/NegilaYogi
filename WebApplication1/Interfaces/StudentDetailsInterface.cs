using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
   public interface StudentDetailsInterface 
    {
        PointsReportDTO getdetails(PointsReportDTO data);
        Task<PointsReportDTO> Getreportdetails(PointsReportDTO data);

    }
}
