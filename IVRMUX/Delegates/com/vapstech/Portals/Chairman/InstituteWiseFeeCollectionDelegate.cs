
using System;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class InstituteWiseFeeCollectionDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<InstituteWiseFeeCollectionDTO, InstituteWiseFeeCollectionDTO> COMMM = new CommonDelegate<InstituteWiseFeeCollectionDTO, InstituteWiseFeeCollectionDTO>();
        public InstituteWiseFeeCollectionDTO Getdetails(InstituteWiseFeeCollectionDTO data)
        {
            return COMMM.POSTPORTALData(data, "InstituteWiseFeeCollectionFacade/Getdetails/");
        }

      
        public InstituteWiseFeeCollectionDTO Getsectioncount(InstituteWiseFeeCollectionDTO data)
        {
            return COMMM.POSTPORTALData(data, "InstituteWiseFeeCollectionFacade/Getsectioncount/");
        }
    }
}
