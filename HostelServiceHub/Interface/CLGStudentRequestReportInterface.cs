using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Interface
{
    public interface CLGStudentRequestReportInterface
    {
        CLGStudentReportDTO getdata(CLGStudentReportDTO data);
        Task<CLGStudentReportDTO> getreport(CLGStudentReportDTO data);
        Task<CLGStudentReportDTO> getconfirmreport(CLGStudentReportDTO data);
    }
}
