using PreadmissionDTOs.com.vaps.Portals.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Principal.Interfaces
{
    public interface SmsEmailDetailsInterface
    {
        SmsEmailDetailsDTO getdata(SmsEmailDetailsDTO obj);

        SmsEmailDetailsDTO Getreportdetails(SmsEmailDetailsDTO data);
        SmsEmailDetailsDTO Getreportdetails1(SmsEmailDetailsDTO data);
    }
}
