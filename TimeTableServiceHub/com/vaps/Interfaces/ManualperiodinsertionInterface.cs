using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
    public interface ManualperiodinsertionInterface
    {
        ManualperiodinsertionDTO getdetails(int id);
        ManualperiodinsertionDTO get_catg(ManualperiodinsertionDTO objcategory);
        ManualperiodinsertionDTO getclass_catg(ManualperiodinsertionDTO objcategory);
        ManualperiodinsertionDTO getreport(ManualperiodinsertionDTO objcategory);
        ManualperiodinsertionDTO getpossiblePeriod(ManualperiodinsertionDTO objcategory);
        ManualperiodinsertionDTO savedetail(ManualperiodinsertionDTO objperiod);
    }
}
