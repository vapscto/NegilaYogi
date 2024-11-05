using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
   public interface IVRM_PushNotificationInterface
    {
        IVRM_PushNotificationDTO savedetail(IVRM_PushNotificationDTO data);
        Task<IVRM_PushNotificationDTO> Getdetails(IVRM_PushNotificationDTO data);
        IVRM_PushNotificationDTO deactivate(IVRM_PushNotificationDTO data);
        IVRM_PushNotificationDTO editnoticestf(IVRM_PushNotificationDTO data);
        IVRM_PushNotificationDTO editnoticestud(IVRM_PushNotificationDTO data);
        IVRM_PushNotificationDTO Deactivatemain(IVRM_PushNotificationDTO data);
        IVRM_PushNotificationDTO Deactivatestud(IVRM_PushNotificationDTO data);
        IVRM_PushNotificationDTO get_modeldata(IVRM_PushNotificationDTO data);

        
    }
}
