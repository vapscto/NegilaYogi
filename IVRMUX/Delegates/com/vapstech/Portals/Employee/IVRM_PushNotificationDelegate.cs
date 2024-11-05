using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Portals.Employee
{
    public class IVRM_PushNotificationDelegate
    {
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<IVRM_PushNotificationDTO, IVRM_PushNotificationDTO> COMMM = new CommonDelegate<IVRM_PushNotificationDTO, IVRM_PushNotificationDTO>();
        public IVRM_PushNotificationDTO savedetail(IVRM_PushNotificationDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_PushNotificationFacade/savedetail/");
        }
        public IVRM_PushNotificationDTO Getdetails(IVRM_PushNotificationDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_PushNotificationFacade/Getdetails/");
        }
        public IVRM_PushNotificationDTO deactivate(IVRM_PushNotificationDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_PushNotificationFacade/deactivate/");
        }
       
        public IVRM_PushNotificationDTO editnoticestf(IVRM_PushNotificationDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_PushNotificationFacade/editnoticestf/");
        }

        public IVRM_PushNotificationDTO editnoticestud(IVRM_PushNotificationDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_PushNotificationFacade/editnoticestud/");
        }

        public IVRM_PushNotificationDTO Deactivatemain(IVRM_PushNotificationDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_PushNotificationFacade/Deactivatemain/");
        }

        public IVRM_PushNotificationDTO Deactivatestud(IVRM_PushNotificationDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_PushNotificationFacade/Deactivatestud/");
        }
        public IVRM_PushNotificationDTO get_modeldata(IVRM_PushNotificationDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_PushNotificationFacade/get_modeldata/");
        }

        
    }
}
