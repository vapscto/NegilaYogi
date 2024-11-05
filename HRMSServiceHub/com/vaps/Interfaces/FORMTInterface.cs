using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
   public interface FORMTInterface
    {
    FORMTDTO getBasicData(FORMTDTO dto);

    //FilterEmployeeData

    FORMTDTO FilterEmployeeData(FORMTDTO dto);
    FORMTDTO getEmployeedetailsBySelection(FORMTDTO dto);
  }
}
