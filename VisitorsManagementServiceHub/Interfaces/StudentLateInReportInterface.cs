using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Interfaces
{
   public interface StudentLateInReportInterface
    {
        LateInStudent_DTO loaddata(LateInStudent_DTO data);
        Task<LateInStudent_DTO> getReport(LateInStudent_DTO data);

    }
}
