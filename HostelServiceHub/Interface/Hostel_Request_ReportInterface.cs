using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Interface
{
   public interface Hostel_Request_ReportInterface
    {
        Hostel_Request_ReportDTO getdata(Hostel_Request_ReportDTO data);
        Task <Hostel_Request_ReportDTO> getreport(Hostel_Request_ReportDTO data);
        Task <Hostel_Request_ReportDTO> getconfirmreport(Hostel_Request_ReportDTO data);
    }
}
