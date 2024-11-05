using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface StudentSmartCardLogReportInterface
    {
      Task<StudentSmartCardLogReportDTO> getdetails(StudentSmartCardLogReportDTO id);

        StudentSmartCardLogReportDTO getstuddet(StudentSmartCardLogReportDTO data);

      Task<StudentSmartCardLogReportDTO> Getreportdetails(StudentSmartCardLogReportDTO data);
    }
}
