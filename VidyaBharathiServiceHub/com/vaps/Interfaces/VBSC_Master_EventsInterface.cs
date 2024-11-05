using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaBharathiServiceHub.com.vaps.Interfaces
{
    public interface VBSC_Master_EventsInterface
    {
        VBSC_Master_EventsDTO getloaddata(VBSC_Master_EventsDTO data);
        VBSC_Master_EventsDTO savedetails(VBSC_Master_EventsDTO data);
        VBSC_Master_EventsDTO deactive(VBSC_Master_EventsDTO data);

    }
}
