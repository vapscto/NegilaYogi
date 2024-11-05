using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
   public interface FORMNO15GInterface
    {
    FORMNO15GDTO getBasicData(FORMNO15GDTO dto);

    //FilterEmployeeData

    FORMNO15GDTO FilterEmployeeData(FORMNO15GDTO dto);
    FORMNO15GDTO getEmployeedetailsBySelection(FORMNO15GDTO dto);
  }
}
