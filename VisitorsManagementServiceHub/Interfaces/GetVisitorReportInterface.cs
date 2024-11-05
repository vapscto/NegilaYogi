using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Interfaces
{
    public interface GetVisitorReportInterface
    {
        Task<GetVisitorReportDTO> report(GetVisitorReportDTO data);
    }
}
