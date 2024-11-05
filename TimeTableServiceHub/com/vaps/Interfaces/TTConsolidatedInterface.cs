using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
    public interface TTConsolidatedInterface
    {


        TTConsolidatedDTO getalldetails(int id);
        TTConsolidatedDTO getreport(TTConsolidatedDTO objcategory);
        TTConsolidatedDTO getclass_catg(TTConsolidatedDTO objcatg);

    }
}
