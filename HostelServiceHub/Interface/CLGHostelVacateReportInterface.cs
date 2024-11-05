using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Interface
{
    public interface CLGHostelVacateReportInterface
    {
        CLGHostelVacateReportDTO loaddata(CLGHostelVacateReportDTO data);
        Task<CLGHostelVacateReportDTO> get_report(CLGHostelVacateReportDTO data);
        CLGHostelVacateReportDTO get_Studentlist(CLGHostelVacateReportDTO data);
    }
}

