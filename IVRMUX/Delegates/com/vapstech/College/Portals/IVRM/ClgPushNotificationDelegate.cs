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
using PreadmissionDTOs.com.vaps.College.Portals.IVRM;

namespace corewebapi18072016.Delegates.com.vapstech.College.Portals.IVRM
{
    public class ClgPushNotificationDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgPushNotificationDTO, ClgPushNotificationDTO> COMMM = new CommonDelegate<ClgPushNotificationDTO, ClgPushNotificationDTO>();
        public ClgPushNotificationDTO getloaddata(ClgPushNotificationDTO data)
        {     
            return COMMM.CLGPortalPOSTData(data, "ClgPushNotificationFacade/getloaddata/");
        }      
        public ClgPushNotificationDTO savedata(ClgPushNotificationDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgPushNotificationFacade/savedata/");
        }
        public ClgPushNotificationDTO getNotificationdetails(ClgPushNotificationDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgPushNotificationFacade/getNotificationdetails/");
        }
        


    }
}
