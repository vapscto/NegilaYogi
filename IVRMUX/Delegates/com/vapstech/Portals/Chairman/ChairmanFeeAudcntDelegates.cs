
using System;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class ChairmanFeeAudcntDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ChairmanFeeAudcntDTO, ChairmanFeeAudcntDTO> COMMM = new CommonDelegate<ChairmanFeeAudcntDTO, ChairmanFeeAudcntDTO>();
        public ChairmanFeeAudcntDTO Getdetails(ChairmanFeeAudcntDTO data)
        {
            return COMMM.POSTPORTALData(data, "ChairmanFeeAudcntFacade/Getdetails/");
        }

      
       
        public ChairmanFeeAudcntDTO Getsectionpop(ChairmanFeeAudcntDTO data)
        {
            return COMMM.POSTPORTALData(data, "ChairmanFeeAudcntFacade/Getsectionpop/");
        }
    }
    
}
