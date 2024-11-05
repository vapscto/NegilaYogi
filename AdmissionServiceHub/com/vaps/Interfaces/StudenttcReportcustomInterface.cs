using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
   public interface StudenttcReportcustomInterface 
    {
        Task<StudentAttendanceReportDTO> getInitailData(int id);
        StudentAttendanceReportDTO getstudlist(int id);
        StudentAttendanceReportDTO getstuddetails(StudentAttendanceReportDTO data);
        StudentAttendanceReportDTO  getTCdata(StudentAttendanceReportDTO data);

        StudentAttendanceReportDTO getTcdetailsbwmc(StudentAttendanceReportDTO data);

        
    }
}
