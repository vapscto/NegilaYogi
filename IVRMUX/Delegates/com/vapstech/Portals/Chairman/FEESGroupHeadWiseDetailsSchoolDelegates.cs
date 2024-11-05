
using System;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class FEESGroupHeadWiseDetailsSchoolDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<FEESGroupHeadWiseDetailsSchoolDTO, FEESGroupHeadWiseDetailsSchoolDTO> COMMM = new CommonDelegate<FEESGroupHeadWiseDetailsSchoolDTO, FEESGroupHeadWiseDetailsSchoolDTO>();
        public FEESGroupHeadWiseDetailsSchoolDTO Getdetails(FEESGroupHeadWiseDetailsSchoolDTO data)
        {
            return COMMM.POSTPORTALData(data, "FEESGroupHeadWiseDetailsSchoolFacade/Getdetails/");
        }

        public FEESGroupHeadWiseDetailsSchoolDTO Getsectioncount(FEESGroupHeadWiseDetailsSchoolDTO data)
        {
            return COMMM.POSTPORTALData(data, "FEESGroupHeadWiseDetailsSchoolFacade/Getsectioncount/");
        }

        public FEESGroupHeadWiseDetailsSchoolDTO Getgroupclasscount(FEESGroupHeadWiseDetailsSchoolDTO data)
        {
            return COMMM.POSTPORTALData(data, "FEESGroupHeadWiseDetailsSchoolFacade/Getgroupclasscount/");
        }
    }
    
}
