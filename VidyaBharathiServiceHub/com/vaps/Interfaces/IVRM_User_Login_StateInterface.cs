using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaBharathiServiceHub.com.vaps.Interfaces
{
   public interface IVRM_User_Login_StateInterface
    {
        IVRM_User_Login_StateDTO loaddata(IVRM_User_Login_StateDTO data);
        IVRM_User_Login_StateDTO savedata(IVRM_User_Login_StateDTO data);
        IVRM_User_Login_StateDTO deactive(IVRM_User_Login_StateDTO data);
        IVRM_User_Login_StateDTO edit(IVRM_User_Login_StateDTO data);
    }
}
