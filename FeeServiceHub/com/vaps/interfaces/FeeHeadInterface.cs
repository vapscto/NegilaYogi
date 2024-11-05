using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeHeadInterface
    {
        FeeHeadDTO SaveGroupData(FeeHeadDTO org);
        FeeHeadDTO changeorderData(FeeHeadDTO org);
        FeeHeadDTO EditgroupDetails(int id);
        FeeHeadDTO getdetails(int id);
        FeeHeadDTO GetGroupSearchData(FeeHeadDTO mas);
        FeeHeadDTO getpageedit(int id);
        FeeHeadDTO deleterec(int id);
        FeeHeadDTO deactivate(FeeHeadDTO id);
    }
}
