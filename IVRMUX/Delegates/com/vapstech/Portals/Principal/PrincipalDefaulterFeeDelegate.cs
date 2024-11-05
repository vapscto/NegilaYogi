
using System;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using CommonLibrary;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Principal
{
    public class PrincipalDefaulterFeeDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<PrincipalDefaulterFeeDTO, PrincipalDefaulterFeeDTO> COMMM = new CommonDelegate<PrincipalDefaulterFeeDTO, PrincipalDefaulterFeeDTO>();
        public PrincipalDefaulterFeeDTO Getdetails(PrincipalDefaulterFeeDTO data)
        {
            return COMMM.POSTPORTALData(data, "PrincipalDefaulterFeeFacade/Getdetails/");
        }

        public PrincipalDefaulterFeeDTO getclass(PrincipalDefaulterFeeDTO data)
        {
            return COMMM.POSTPORTALData(data, "PrincipalDefaulterFeeFacade/getclass/");
        }
        public PrincipalDefaulterFeeDTO Getsection(PrincipalDefaulterFeeDTO data)
        {
            return COMMM.POSTPORTALData(data, "PrincipalDefaulterFeeFacade/Getsection/");
        }
        public PrincipalDefaulterFeeDTO Getsectioncount(PrincipalDefaulterFeeDTO data)
        {
            return COMMM.POSTPORTALData(data, "PrincipalDefaulterFeeFacade/Getsectioncount/");
        }
        public PrincipalDefaulterFeeDTO Getreport(PrincipalDefaulterFeeDTO data)
        {
            return COMMM.POSTPORTALData(data, "PrincipalDefaulterFeeFacade/Getreport/");
        }

        public PrincipalDefaulterFeeDTO Getstudentdetails(PrincipalDefaulterFeeDTO data)
        {
            return COMMM.POSTPORTALData(data, "PrincipalDefaulterFeeFacade/Getstudentdetails/");
        }



    }


}
