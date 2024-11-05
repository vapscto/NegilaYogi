using CommonLibrary;
using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VidyaBharathi
{
    public class VBSC_EventsDelegate
    {
        CommonDelegate<VBSC_EventsDTO, VBSC_EventsDTO> COMMC = new CommonDelegate<VBSC_EventsDTO, VBSC_EventsDTO>();

        public VBSC_EventsDTO getloaddata(VBSC_EventsDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_EventsFacade/getloaddata/");
        }
        public VBSC_EventsDTO savedetails(VBSC_EventsDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_EventsFacade/savedetails/");
        }
        public VBSC_EventsDTO deactive(VBSC_EventsDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_EventsFacade/deactive/");
        }
    
    }
}
