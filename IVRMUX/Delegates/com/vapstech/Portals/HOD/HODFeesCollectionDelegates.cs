using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.HOD
{
    public class HODFeesCollectionDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<FEESOverAllStatusSchoolDTO, FEESOverAllStatusSchoolDTO> COMMM = new CommonDelegate<FEESOverAllStatusSchoolDTO, FEESOverAllStatusSchoolDTO>();
        public FEESOverAllStatusSchoolDTO Getdetails(FEESOverAllStatusSchoolDTO data)
        {
            return COMMM.POSTPORTALData(data, "HODFeesCollectionFacade/Getdetails");
        }

        public FEESOverAllStatusSchoolDTO Getsectioncount(FEESOverAllStatusSchoolDTO data)
        {
            return COMMM.POSTPORTALData(data, "HODFeesCollectionFacade/Getsectioncount");
        }

    }
}
