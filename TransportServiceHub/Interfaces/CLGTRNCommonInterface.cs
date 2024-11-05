using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
   public  interface CLGTRNCommonInterface
    {
     
        CLGTRNCommonDTO get_course(CLGTRNCommonDTO data);
        CLGTRNCommonDTO getBranch(CLGTRNCommonDTO data);
        CLGTRNCommonDTO get_semister(CLGTRNCommonDTO data);
        CLGTRNCommonDTO get_section(CLGTRNCommonDTO data);
        CLGTRNCommonDTO get_location(CLGTRNCommonDTO data);

    }
}
