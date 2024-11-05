using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface SwimmingAttendanceInterface
    {
        SwimmingAttendanceDTO loaddata(SwimmingAttendanceDTO data);
        SwimmingAttendanceDTO onchnageyear(SwimmingAttendanceDTO data);
        SwimmingAttendanceDTO onchangeclass(SwimmingAttendanceDTO data);
        SwimmingAttendanceDTO search(SwimmingAttendanceDTO data);
        SwimmingAttendanceDTO save(SwimmingAttendanceDTO data);
    }
}
