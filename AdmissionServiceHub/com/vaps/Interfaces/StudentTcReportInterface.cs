using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
  public  interface StudentTcReportInterface
    {
        StudentTcReportDTO getdetails(StudentTcReportDTO id);
        Task<StudentTcReportDTO> Getdata(StudentTcReportDTO dto);
        StudentTcReportDTO getclass(StudentTcReportDTO dto);
        
        StudentTcReportDTO getsecton(StudentTcReportDTO id);
        
    }
}
