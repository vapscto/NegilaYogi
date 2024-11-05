using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{

    public  interface ClassWiseTTInterface
    {
        TTClassWiseTTDTO getreport(TTClassWiseTTDTO objcategory);
        TTClassWiseTTDTO getclass_catg(TTClassWiseTTDTO objcategory);
        TTClassWiseTTDTO getdetails(int id);
      

    }
}
