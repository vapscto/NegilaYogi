using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface MasterChapterVIInterface
    {
        MasterChapterVIDTO getBasicData(MasterChapterVIDTO dto);
        MasterChapterVIDTO SaveUpdate(MasterChapterVIDTO dto);
        MasterChapterVIDTO editData(int id);

        MasterChapterVIDTO deactivate(MasterChapterVIDTO dto);

        MasterChapterVIDTO validateordernumber(MasterChapterVIDTO data);
    }
}
