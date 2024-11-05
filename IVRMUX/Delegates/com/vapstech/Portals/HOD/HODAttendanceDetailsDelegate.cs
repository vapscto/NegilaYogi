using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.Student;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Chirman;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.HOD
{
    public class HODAttendanceDetailsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ADMAttendenceDTO, ADMAttendenceDTO> COMMM = new CommonDelegate<ADMAttendenceDTO, ADMAttendenceDTO>();
       // CommonDelegate<ADMAttendenceDTO, ADMAttendenceDTO> COMMM = new CommonDelegate<ADMAttendenceDTO, ADMAttendenceDTO>();
        public ADMAttendenceDTO Getdetails(ADMAttendenceDTO data)
        {
            return COMMM.POSTPORTALData(data, "HODAttendanceDetailsFacade/Getdetails/");
        }

        public ADMAttendenceDTO getclass(ADMAttendenceDTO data)
        {
            return COMMM.POSTPORTALData(data, "HODAttendanceDetailsFacade/getclass/");
        }
        public ADMAttendenceDTO Getsection(ADMAttendenceDTO data)
        {
            return COMMM.POSTPORTALData(data, "HODAttendanceDetailsFacade/Getsection/");
        }
        public ADMAttendenceDTO GetAttendence(ADMAttendenceDTO data)
        {
            return COMMM.POSTPORTALData(data, "HODAttendanceDetailsFacade/GetAttendence/");
        }

        public ADMAttendenceDTO GetIndividualAttendence(ADMAttendenceDTO data)
        {
            return COMMM.POSTPORTALData(data, "HODAttendanceDetailsFacade/GetIndividualAttendence/");
        }


    }
}
