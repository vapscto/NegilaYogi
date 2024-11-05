using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface MasterNarrationInterface
    {
        MasterNarrationDTO SaveGroupData(MasterNarrationDTO org);

        MasterNarrationDTO EditgroupDetails(int id);
        MasterNarrationDTO getdetails(int id);
        MasterNarrationDTO GetGroupSearchData(MasterNarrationDTO mas);
        MasterNarrationDTO getpageedit(int id);
        MasterNarrationDTO deactivate(MasterNarrationDTO id);
  
    }
}
