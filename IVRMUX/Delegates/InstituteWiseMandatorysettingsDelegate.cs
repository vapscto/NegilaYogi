using CommonLibrary;
using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates
{
    public class InstituteWiseMandatorysettingsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<IVRM_Mandatory_Setting_IWDTO, IVRM_Mandatory_Setting_IWDTO> COMMM = new CommonDelegate<IVRM_Mandatory_Setting_IWDTO, IVRM_Mandatory_Setting_IWDTO>();

        public IVRM_Mandatory_Setting_IWDTO onloadgetdetails(IVRM_Mandatory_Setting_IWDTO dto)
        {
            return COMMM.POSTData(dto, "InstituteWiseMandatorysettingsFacade/onloadgetdetails");
        }

        public IVRM_Mandatory_Setting_IWDTO getPagedetailsBySelection(IVRM_Mandatory_Setting_IWDTO maspage)
        {
            return COMMM.POSTData(maspage, "InstituteWiseMandatorysettingsFacade/getPagedetailsBySelection/");
        }

        public IVRM_Mandatory_Setting_IWDTO savedetails(IVRM_Mandatory_Setting_IWDTO maspage)
        {
            return COMMM.POSTData(maspage, "InstituteWiseMandatorysettingsFacade/");
        }

        public IVRM_Mandatory_Setting_IWDTO getRecorddetailsById(IVRM_Mandatory_Setting_IWDTO maspage)
        {
            return COMMM.POSTData(maspage, "InstituteWiseMandatorysettingsFacade/getRecordById/");

        }
        public IVRM_Mandatory_Setting_IWDTO deleterec(IVRM_Mandatory_Setting_IWDTO maspage)
        {
            return COMMM.POSTData(maspage, "InstituteWiseMandatorysettingsFacade/deactivateRecordById/");
        }
    }
}
