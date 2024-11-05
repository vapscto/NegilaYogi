using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

//PreadmissionDTOs.com.vaps.admission


namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface EMAILSMSTemplateSettingInterface
    {
        EMAILSMSTemplateSettingDTO MasterActivityData(EMAILSMSTemplateSettingDTO mas);     

        EMAILSMSTemplateSettingDTO MasterDeleteModulesData(int ID);

        EMAILSMSTemplateSettingDTO GetSelectedRowDetails(int ID);

        EMAILSMSTemplateSettingDTO Getdetails(EMAILSMSTemplateSettingDTO data);
    }
}
