using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface AttendanceLPInterface
    {
       AttendanceLPDTO getDataByTypeSelected(AttendanceLPDTO dto);
        AttendanceLPDTO SaveData(AttendanceLPDTO adtto);
        AttendanceLPDTO GetInitialData(long MIID);
        AttendanceLPDTO GetEditData(AttendanceLPDTO attdto);
        AttendanceLPDTO staffwisegrid(AttendanceLPDTO attdto);
        AttendanceLPDTO getyear(AttendanceLPDTO attdto);
        AttendanceLPDTO deleteAttPrivileges(AttendanceLPDTO obj);
        

    }
}
