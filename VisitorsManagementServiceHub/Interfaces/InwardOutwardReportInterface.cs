using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Interfaces
{
    public interface InwardOutwardReportInterface
    {
        Task<InwardOutwardReportDTO> report(InwardOutwardReportDTO data);
    }
}
