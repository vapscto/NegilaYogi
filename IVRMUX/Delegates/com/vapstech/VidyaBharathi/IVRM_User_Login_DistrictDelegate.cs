using CommonLibrary;
using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VidyaBharathi
{
    public class IVRM_User_Login_DistrictDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
      
        CommonDelegate<IVRM_User_Login_DistrictDTO, IVRM_User_Login_DistrictDTO> COMMC = new CommonDelegate<IVRM_User_Login_DistrictDTO, IVRM_User_Login_DistrictDTO>();
        public IVRM_User_Login_DistrictDTO loaddata(IVRM_User_Login_DistrictDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "IVRM_User_Login_DistrictFacade/loaddata/");
        }

        //POSTDataClubManagement
        public IVRM_User_Login_DistrictDTO savedata(IVRM_User_Login_DistrictDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "IVRM_User_Login_DistrictFacade/savedata/");
        }
        public IVRM_User_Login_DistrictDTO deactive(IVRM_User_Login_DistrictDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "IVRM_User_Login_DistrictFacade/deactive/");
        }
    }
}
