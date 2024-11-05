using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaBharathiServiceHub.com.vaps.Interfaces
{
   public interface IVRM_User_Login_DistrictInterface
    {
        IVRM_User_Login_DistrictDTO loaddata(IVRM_User_Login_DistrictDTO data);
        IVRM_User_Login_DistrictDTO savedata(IVRM_User_Login_DistrictDTO data);
        IVRM_User_Login_DistrictDTO deactive(IVRM_User_Login_DistrictDTO data);
    }
}
