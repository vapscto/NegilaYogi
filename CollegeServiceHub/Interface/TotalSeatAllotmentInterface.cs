using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface TotalSeatAllotmentInterface
    {
        TotalSeatAllotmentDTO getdetails(TotalSeatAllotmentDTO data);
        TotalSeatAllotmentDTO onselectAcdYear(TotalSeatAllotmentDTO data);
        TotalSeatAllotmentDTO onselectCourse(TotalSeatAllotmentDTO data);
        TotalSeatAllotmentDTO onreport(TotalSeatAllotmentDTO data);

    }
}
