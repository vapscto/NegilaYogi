
using System;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class LateInDetailsDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<LateInDetailsDTO, LateInDetailsDTO> COMMM = new CommonDelegate<LateInDetailsDTO, LateInDetailsDTO>();
        public LateInDetailsDTO getalldetails(LateInDetailsDTO data)
        {
            return COMMM.POSTPORTALData(data, "LateInDetailsFacade/getalldetails/");
        }

//          
  }
}
