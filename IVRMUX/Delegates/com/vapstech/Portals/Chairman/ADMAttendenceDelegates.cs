
using System;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class ADMAttendenceDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ADMAttendenceDTO, ADMAttendenceDTO> COMMM = new CommonDelegate<ADMAttendenceDTO, ADMAttendenceDTO>();
        public ADMAttendenceDTO Getdetails(ADMAttendenceDTO data)
        {
            return COMMM.POSTPORTALData(data, "ADMAttendenceFacade/Getdetails/");
        }

        public ADMAttendenceDTO getclass(ADMAttendenceDTO data)
        {
            return COMMM.POSTPORTALData(data, "ADMAttendenceFacade/getclass/");
        }
        public ADMAttendenceDTO Getsection(ADMAttendenceDTO data)
        {
            return COMMM.POSTPORTALData(data, "ADMAttendenceFacade/Getsection/");
        }
        public ADMAttendenceDTO GetAttendence(ADMAttendenceDTO data)
        {
            return COMMM.POSTPORTALData(data, "ADMAttendenceFacade/GetAttendence/");
        }

        public ADMAttendenceDTO GetIndividualAttendence(ADMAttendenceDTO data)
        {
            return COMMM.POSTPORTALData(data, "ADMAttendenceFacade/GetIndividualAttendence/");
        }



        
    }
}
