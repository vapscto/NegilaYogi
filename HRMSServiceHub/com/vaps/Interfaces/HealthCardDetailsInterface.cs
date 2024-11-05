using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Interfaces
{
   public interface HealthCardDetailsInterface
    {
        HealthCardDetailsDTO loaddata(HealthCardDetailsDTO data);
        HealthCardDetailsDTO SaveDetails(HealthCardDetailsDTO data);
        //OnChangeEmployee
        HealthCardDetailsDTO OnChangeEmployee(HealthCardDetailsDTO data);
        //Savemaster
        HealthCardMasterDTO Savemaster(HealthCardMasterDTO data);
        //editmaster
        HealthCardMasterDTO editmaster(HealthCardMasterDTO data);
        //deactiveM
        HealthCardMasterDTO deactiveM(HealthCardMasterDTO data);
    }
}
