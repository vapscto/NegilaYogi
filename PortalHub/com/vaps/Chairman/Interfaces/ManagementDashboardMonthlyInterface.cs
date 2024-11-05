using PreadmissionDTOs.com.vaps.Portals.Chirman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Chairman.Interfaces
{
   public interface ManagementDashboardMonthlyInterface
    {
        ManagementDashboardMonthlyDTO Getdetails(ManagementDashboardMonthlyDTO data);
        ManagementDashboardMonthlyDTO getreport(ManagementDashboardMonthlyDTO data);
    }
}
