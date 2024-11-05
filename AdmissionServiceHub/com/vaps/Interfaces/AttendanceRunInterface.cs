using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface AttendanceRunInterface
    {
        AttendanceRunDTO loaddata(AttendanceRunDTO data);
        AttendanceRunDTO savedetails(AttendanceRunDTO data);
        AttendanceRunDTO griddetails(AttendanceRunDTO data);
    }
}
