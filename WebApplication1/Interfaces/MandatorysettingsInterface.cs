using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
   public interface MandatorysettingsInterface
    {
        IVRM_Mandatory_SettingDTO getBasicData(IVRM_Mandatory_SettingDTO dto);
        IVRM_Mandatory_SettingDTO SaveUpdate(IVRM_Mandatory_SettingDTO dto);

        IVRM_Mandatory_SettingDTO getPagedetailsBySelection(IVRM_Mandatory_SettingDTO dto);
        IVRM_Mandatory_SettingDTO editData(int id);

        IVRM_Mandatory_SettingDTO deactivate(IVRM_Mandatory_SettingDTO dto);
    }
}
