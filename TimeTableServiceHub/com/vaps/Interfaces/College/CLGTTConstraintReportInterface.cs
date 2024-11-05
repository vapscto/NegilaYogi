using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces.College
{
   public interface CLGTTConstraintReportInterface
    {
        CLGTTConstraintReportDTO getalldetails(int id);
        CLGTTConstraintReportDTO getpagedetails(CLGTTConstraintReportDTO data);
    }
}
