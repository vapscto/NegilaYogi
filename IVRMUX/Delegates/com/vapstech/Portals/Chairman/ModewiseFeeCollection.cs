
using System;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class ModewiseFeeCollectionDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ModewiseFeeCollectionDTO, ModewiseFeeCollectionDTO> COMMM = new CommonDelegate<ModewiseFeeCollectionDTO, ModewiseFeeCollectionDTO>();
        public ModewiseFeeCollectionDTO Getdetails(ModewiseFeeCollectionDTO data)
        {
            return COMMM.POSTPORTALData(data, "ModewiseFeeCollectionFacade/Getdetails/");
        }

        public ModewiseFeeCollectionDTO Getsectioncount(ModewiseFeeCollectionDTO data)
        {
            return COMMM.POSTPORTALData(data, "ModewiseFeeCollectionFacade/Getsectioncount/");
        }
        

    }
}
