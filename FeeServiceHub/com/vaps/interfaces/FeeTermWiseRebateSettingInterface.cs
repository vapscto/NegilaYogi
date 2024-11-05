using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeTermWiseRebateSettingInterface
    {
        FeeTermWiseRebateSettingDTO SaveGroupData(FeeTermWiseRebateSettingDTO org);

        FeeTermWiseRebateSettingDTO EditgroupDetails(int id);
        FeeTermWiseRebateSettingDTO getdetails(int id);
        FeeTermWiseRebateSettingDTO GetGroupSearchData(FeeTermWiseRebateSettingDTO mas);
        FeeTermWiseRebateSettingDTO getpageedit(int id);
        FeeTermWiseRebateSettingDTO deactivate(FeeTermWiseRebateSettingDTO id);

    }
}
