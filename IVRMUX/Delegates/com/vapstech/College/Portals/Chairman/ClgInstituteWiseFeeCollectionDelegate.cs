using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Portals.Chairman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates
{
    public class ClgInstituteWiseFeeCollectionDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgInstituteWiseFeeCollectionDTO, ClgInstituteWiseFeeCollectionDTO> COMMM = new CommonDelegate<ClgInstituteWiseFeeCollectionDTO, ClgInstituteWiseFeeCollectionDTO>();
        public ClgInstituteWiseFeeCollectionDTO Getdetails(ClgInstituteWiseFeeCollectionDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgInstituteWiseFeeCollectionFacade/Getdetails/");
        }


        public ClgInstituteWiseFeeCollectionDTO Getsectioncount(ClgInstituteWiseFeeCollectionDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgInstituteWiseFeeCollectionFacade/Getsectioncount/");
        }
    }
}
