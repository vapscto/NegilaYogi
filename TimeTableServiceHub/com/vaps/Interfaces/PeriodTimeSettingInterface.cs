using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{

    public interface PeriodTimeSettingInterface
    {
        TT_Master_Day_Period_TimeDTO savedetail(TT_Master_Day_Period_TimeDTO objcategory);
        TT_Master_Day_Period_TimeDTO getdetails(int id);
        TT_Master_Day_Period_TimeDTO getpageedit(int id);
        TT_Master_Day_Period_TimeDTO deleterec(TT_Master_Day_Period_TimeDTO id);

    }
}
