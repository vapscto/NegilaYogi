using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Chirman;


namespace PortalHub.com.vaps.Chairman.Interfaces
{
    public interface Ch_feedbackInterface
    {
        Ch_feedbackDTO getdata(Ch_feedbackDTO obj);

      
        Ch_feedbackDTO onmonth(Ch_feedbackDTO data);
        
    }
}
