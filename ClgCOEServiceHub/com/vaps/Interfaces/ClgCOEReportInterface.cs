using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.College.COE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClgCOEServiceHub.com.vaps.Interfaces
{
    public interface ClgCOEReportInterface
    {

        ClgMasterCOEDTO getinitialData(int id);
        Task<ClgMasterCOEDTO> getReport(ClgMasterCOEDTO dto);
        ClgMasterCOEDTO mothreport(ClgMasterCOEDTO dto);


    }
}
