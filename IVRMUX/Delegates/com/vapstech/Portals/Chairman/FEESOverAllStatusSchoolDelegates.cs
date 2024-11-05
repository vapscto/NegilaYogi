
using System;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class FEESOverAllStatusSchoolDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<FEESOverAllStatusSchoolDTO, FEESOverAllStatusSchoolDTO> COMMM = new CommonDelegate<FEESOverAllStatusSchoolDTO, FEESOverAllStatusSchoolDTO>();
        public FEESOverAllStatusSchoolDTO Getdetails(FEESOverAllStatusSchoolDTO data)
        {
            return COMMM.POSTPORTALData(data, "FEESOverAllStatusSchoolFacade/Getdetails/");
        }

        public FEESOverAllStatusSchoolDTO Getsectioncount(FEESOverAllStatusSchoolDTO data)
        {
            return COMMM.POSTPORTALData(data, "FEESOverAllStatusSchoolFacade/Getsectioncount/");
        }
        

    }
}
