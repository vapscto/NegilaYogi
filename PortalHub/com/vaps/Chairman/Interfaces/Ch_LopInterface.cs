using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Chirman;


namespace PortalHub.com.vaps.Chairman.Interfaces
{
    public interface Ch_LopInterface
    {
        Ch_LopDTO getdata(Ch_LopDTO obj);

      
        Ch_LopDTO onmonth(Ch_LopDTO data);
        
    }
}
