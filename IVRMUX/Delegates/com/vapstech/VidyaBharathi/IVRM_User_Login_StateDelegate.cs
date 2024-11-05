using CommonLibrary;
using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VidyaBharathi
{
    public class IVRM_User_Login_StateDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
      
        CommonDelegate<IVRM_User_Login_StateDTO, IVRM_User_Login_StateDTO> COMMC = new CommonDelegate<IVRM_User_Login_StateDTO, IVRM_User_Login_StateDTO>();
        public IVRM_User_Login_StateDTO loaddata(IVRM_User_Login_StateDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "IVRM_User_Login_StateFacade/loaddata/");
        }

        //POSTDataClubManagement
        public IVRM_User_Login_StateDTO savedata(IVRM_User_Login_StateDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "IVRM_User_Login_StateFacade/savedata/");
        }
        public IVRM_User_Login_StateDTO deactive(IVRM_User_Login_StateDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "IVRM_User_Login_StateFacade/deactive/");
        }

        public IVRM_User_Login_StateDTO edit(IVRM_User_Login_StateDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "IVRM_User_Login_StateFacade/edit/");
        }
    }
}
