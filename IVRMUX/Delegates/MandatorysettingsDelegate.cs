using CommonLibrary;
using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates
{
    public class MandatorysettingsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<IVRM_Mandatory_SettingDTO, IVRM_Mandatory_SettingDTO> COMMM = new CommonDelegate<IVRM_Mandatory_SettingDTO, IVRM_Mandatory_SettingDTO>();

        public IVRM_Mandatory_SettingDTO onloadgetdetails(IVRM_Mandatory_SettingDTO dto)
        {
            return COMMM.POSTData(dto, "MandatorysettingsFacade/onloadgetdetails");
        }

        public IVRM_Mandatory_SettingDTO getPagedetailsBySelection(IVRM_Mandatory_SettingDTO maspage)
        {
            return COMMM.POSTData(maspage, "MandatorysettingsFacade/getPagedetailsBySelection/");
        }

        public IVRM_Mandatory_SettingDTO savedetails(IVRM_Mandatory_SettingDTO maspage)
        {
            return COMMM.POSTData(maspage, "MandatorysettingsFacade/");
        }
        public IVRM_Mandatory_SettingDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataById(id, "MandatorysettingsFacade/getRecordById/");
        }
        public IVRM_Mandatory_SettingDTO deleterec(IVRM_Mandatory_SettingDTO maspage)
        {
            return COMMM.POSTData(maspage, "MandatorysettingsFacade/deactivateRecordById/");
        }
    }
}
