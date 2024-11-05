using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Portals.Chairman
{
    public class ManagementDashboardMonthlyDelegate
    {
        CommonDelegate<ManagementDashboardMonthlyDTO, ManagementDashboardMonthlyDTO> COMMM = new CommonDelegate<ManagementDashboardMonthlyDTO, ManagementDashboardMonthlyDTO>();
        public ManagementDashboardMonthlyDTO Getdetails(ManagementDashboardMonthlyDTO data)
        {
            return COMMM.POSTPORTALData(data, "ManagementDashboardMonthlyFacade/Getdetails/");
        }

        public ManagementDashboardMonthlyDTO getreport(ManagementDashboardMonthlyDTO data)
        {
            return COMMM.POSTPORTALData(data, "ManagementDashboardMonthlyFacade/getreport/");
        }
    }
}
