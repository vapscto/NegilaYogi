
using System;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class AllFeeCollectionDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<AllFeeCollectionDTO, AllFeeCollectionDTO> COMMM = new CommonDelegate<AllFeeCollectionDTO, AllFeeCollectionDTO>();
        public AllFeeCollectionDTO Getdetails(AllFeeCollectionDTO data)
        {
            return COMMM.POSTPORTALData(data, "AllFeeCollectionFacade/Getdetails/");
        }

        public AllFeeCollectionDTO Getsectioncount(AllFeeCollectionDTO data)
        {
            return COMMM.POSTPORTALData(data, "AllFeeCollectionFacade/Getsectioncount/");
        }
        

    }
}
