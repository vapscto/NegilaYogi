using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface AttendanceEntryTypeInterface
    {
          AttendanceEntryTypeDTO GetAttendanceEntryTypeData(AttendanceEntryTypeDTO AttendanceEntryTypeDTO);

        AttendanceEntryTypeDTO AttendanceEntryTypeData(AttendanceEntryTypeDTO mas);

        AttendanceEntryTypeDTO AttendanceDeleteEntryTypeDATA(int ID);

        //AttendanceEntryTypeDTO GetSelectedRowDetails(int ID);
        AttendanceEntryTypeDTO GetSelectedRowDetails(AttendanceEntryTypeDTO ID);
    }
}
