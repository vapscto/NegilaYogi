using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeYearlyRebateSettingInterface
    {
        FeeYearlyRedateSettingDTO SaveGroupData(FeeYearlyRedateSettingDTO org);
    
        FeeYearlyRedateSettingDTO EditgroupDetails(int id);
        FeeYearlyRedateSettingDTO getdetails(int id);
        FeeYearlyRedateSettingDTO GetGroupSearchData(FeeYearlyRedateSettingDTO mas);
        FeeYearlyRedateSettingDTO getpageedit(int id);
        FeeYearlyRedateSettingDTO deleterec(int id);
        FeeYearlyRedateSettingDTO deactivate(FeeYearlyRedateSettingDTO id);

    }
}
