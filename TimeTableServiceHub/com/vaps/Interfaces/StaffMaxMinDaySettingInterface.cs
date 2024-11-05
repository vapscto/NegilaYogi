using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
    public interface StaffMaxMinDaySettingInterface
    {
        StaffMaxMinDaySettingDTO getdetails(StaffMaxMinDaySettingDTO data);
        StaffMaxMinDaySettingDTO savedetail(StaffMaxMinDaySettingDTO data);
        StaffMaxMinDaySettingDTO getdetail(int id);
        StaffMaxMinDaySettingDTO deactive(StaffMaxMinDaySettingDTO data);
    }
}
