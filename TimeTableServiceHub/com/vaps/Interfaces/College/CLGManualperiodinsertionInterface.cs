using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
    public interface CLGManualperiodinsertionInterface
    {
        CLGManualperiodinsertionDTO getdetails(CLGManualperiodinsertionDTO data);
        CLGManualperiodinsertionDTO get_catg(CLGManualperiodinsertionDTO objcategory);
        CLGManualperiodinsertionDTO getclass_catg(CLGManualperiodinsertionDTO objcategory);
        CLGManualperiodinsertionDTO getreport(CLGManualperiodinsertionDTO objcategory);
        CLGManualperiodinsertionDTO getpossiblePeriod(CLGManualperiodinsertionDTO objcategory);
        CLGManualperiodinsertionDTO savedetail(CLGManualperiodinsertionDTO objperiod);
    }
}
