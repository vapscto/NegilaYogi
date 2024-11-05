using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
   public interface InstituteWiseMandatorysettingsInterface
    {
        IVRM_Mandatory_Setting_IWDTO getBasicData(IVRM_Mandatory_Setting_IWDTO dto);
        IVRM_Mandatory_Setting_IWDTO SaveUpdate(IVRM_Mandatory_Setting_IWDTO dto);

        IVRM_Mandatory_Setting_IWDTO getPagedetailsBySelection(IVRM_Mandatory_Setting_IWDTO dto);

        IVRM_Mandatory_Setting_IWDTO editData(IVRM_Mandatory_Setting_IWDTO dto);


        IVRM_Mandatory_Setting_IWDTO deactivate(IVRM_Mandatory_Setting_IWDTO dto);
    }
}
