using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.Interfaces
{
    public interface CLGBreakTimeSettingInterface
    {
      
        CLGBreakTimeSettingDTO getmaximumperiodscount(CLGBreakTimeSettingDTO data);
        CLGBreakTimeSettingDTO deactivate(CLGBreakTimeSettingDTO data);
        CLGBreakTimeSettingDTO geteditdetails(CLGBreakTimeSettingDTO data);
        CLGBreakTimeSettingDTO getalldetails(CLGBreakTimeSettingDTO data);
        CLGBreakTimeSettingDTO editDay(CLGBreakTimeSettingDTO data);
        CLGBreakTimeSettingDTO getBranch(CLGBreakTimeSettingDTO data);
        CLGBreakTimeSettingDTO savetimedetail(CLGBreakTimeSettingDTO data);
       
    }
}
