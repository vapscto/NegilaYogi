using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
  public   interface TT_ConsecutiveInterface
    {
        TT_ConsecutiveDTO savedetail(TT_ConsecutiveDTO objcategory);
    
        TT_ConsecutiveDTO getdetails(int id);
        TT_ConsecutiveDTO getpageedit(int id);
        TT_ConsecutiveDTO deleterec(int id);
        TT_ConsecutiveDTO getclass_catg(TT_ConsecutiveDTO objcategory);
        TT_ConsecutiveDTO get_catg(TT_ConsecutiveDTO objcategory);
        TT_ConsecutiveDTO deactivate(TT_ConsecutiveDTO id);
    }
}
