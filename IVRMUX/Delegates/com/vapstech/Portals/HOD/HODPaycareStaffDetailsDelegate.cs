using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.HOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Portals.HOD
{
    public class HODPaycareStaffDetailsDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HODPaycareStaffDetails_DTO, HODPaycareStaffDetails_DTO> COMMM = new CommonDelegate<HODPaycareStaffDetails_DTO, HODPaycareStaffDetails_DTO>();
        public HODPaycareStaffDetails_DTO Getdetails(HODPaycareStaffDetails_DTO data)
        {
            return COMMM.POSTPORTALData(data, "HODPaycareStaffDetailsFacade/Getdetails/");
        }
        public HODPaycareStaffDetails_DTO Getemppop(HODPaycareStaffDetails_DTO data)
        {
            return COMMM.POSTPORTALData(data, "HODPaycareStaffDetailsFacade/Getemppop/");
        }

    }
}
