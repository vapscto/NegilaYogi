using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Interfaces
{
    public interface GatePassReportInterface
    {
        GatePassReportDTO loaddata(GatePassReportDTO data);
        Task<GatePassReportDTO> report(GatePassReportDTO data);
        Task<GatePassReportDTO> reportforMobile(GatePassReportDTO data);
    }
}
