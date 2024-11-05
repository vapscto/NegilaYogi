using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{

    public  interface BreaktimesettingsInterface
    {
        TTBreakTimesettingDTO savedetail(TTBreakTimesettingDTO objcategory);
        TTBreakTimesettingDTO getmaximumperiodscount(TTBreakTimesettingDTO objcategory);
        TTBreakTimesettingDTO getclass_catg(TTBreakTimesettingDTO objcategory);
        TTBreakTimesettingDTO get_catg(TTBreakTimesettingDTO objcategory);
        TTBreakTimesettingDTO getdetails(int id);
      
        TTBreakTimesettingDTO getpageedit(int id);
        TTBreakTimesettingDTO deleterec(int id);
        TTBreakTimesettingDTO deactivate(TTBreakTimesettingDTO id);
    }
}
