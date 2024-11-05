using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Portals;
using PreadmissionDTOs.com.vaps.College.Portals.IVRM;

namespace CollegePortals.com.Student.Interfaces
{
    public interface ClgPushNotificationInterface
    {
        Task<ClgPushNotificationDTO> getloaddata(ClgPushNotificationDTO data);

        ClgPushNotificationDTO savedata(ClgPushNotificationDTO data);
        ClgPushNotificationDTO getNotificationdetails(ClgPushNotificationDTO data);
        


    }
}
