using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.College.Portals.Student;
using CommonLibrary;

namespace corewebapi18072016.Delegates.com.vapstech.College.Portals.Student
{
    public class ClgAttendanceDetailsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgPortalAttendanceDTO, ClgPortalAttendanceDTO> COMMM = new CommonDelegate<ClgPortalAttendanceDTO, ClgPortalAttendanceDTO>();
        public ClgPortalAttendanceDTO getloaddata(ClgPortalAttendanceDTO data)
        {     
            return COMMM.CLGPortalPOSTData(data, "ClgAttendanceDetailsFacade/getloaddata/");
        }
        public ClgPortalAttendanceDTO getAttdata(ClgPortalAttendanceDTO sddto)
        {
            return COMMM.CLGPortalPOSTData(sddto, "ClgAttendanceDetailsFacade/getAttdata/");
        }
    }
}
