using PreadmissionDTOs.com.vaps.College.COE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COEServiceHub.com.vaps.Interfaces
{
    public interface ClgReportCOEInterface
    {
        ClgMasterCOEDTO getinitialData(int id);
        Task<ClgMasterCOEDTO> getReport(ClgMasterCOEDTO dto);
        ClgMasterCOEDTO mothreport(ClgMasterCOEDTO dto);


    }
}
