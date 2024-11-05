using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaBharathiServiceHub.com.vaps.Interfaces
{
    public interface VBSC_EventsInterface
    {
        VBSC_EventsDTO getloaddata(VBSC_EventsDTO data);
        VBSC_EventsDTO savedetails(VBSC_EventsDTO data);
        VBSC_EventsDTO deactive(VBSC_EventsDTO data);

    }
}
