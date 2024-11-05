
using System;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class FEESTodayCollectionDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<FEESTodayCollectionDTO, FEESTodayCollectionDTO> COMMM = new CommonDelegate<FEESTodayCollectionDTO, FEESTodayCollectionDTO>();
        public FEESTodayCollectionDTO Getdetails(FEESTodayCollectionDTO data)
        {
            return COMMM.POSTPORTALData(data, "FEESTodayCollectionFacade/Getdetails/");
        }

      
       
        public FEESTodayCollectionDTO Getsectionpop(FEESTodayCollectionDTO data)
        {
            return COMMM.POSTPORTALData(data, "FEESTodayCollectionFacade/Getsectionpop/");
        }
    }
    
}
