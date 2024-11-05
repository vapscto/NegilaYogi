using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Interface
{
    public interface HostelVacateReportInterface
    {
        HostelVacateReportDTO loaddata(HostelVacateReportDTO data);
         Task<HostelVacateReportDTO> get_report(HostelVacateReportDTO data);
        HostelVacateReportDTO get_Studentlist(HostelVacateReportDTO data);
    }
}
