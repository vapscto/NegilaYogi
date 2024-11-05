using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{    public interface ConstraintReportInterface
    {
        TT_ConstraintReportDTO getalldetails(int id);
        TT_ConstraintReportDTO getpageedit(TT_ConstraintReportDTO id);
    }
}
