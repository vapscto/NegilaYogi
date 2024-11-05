using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
   public interface MarksReportInterface
    {
         MarksReportDTO getdetails(MarksReportDTO data);
        MarksReportDTO schedulelist(MarksReportDTO data);
       MarksReportDTO Getreportdetails(MarksReportDTO data);
        MarksReportDTO Getreportdetailssrkvs(MarksReportDTO data);

    }
}
