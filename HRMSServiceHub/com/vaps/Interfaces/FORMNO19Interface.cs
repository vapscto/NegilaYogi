using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
   public interface FORMNO19Interface
    {
    FORMNO19DTO getBasicData(FORMNO19DTO dto);

    //FilterEmployeeData

    FORMNO19DTO FilterEmployeeData(FORMNO19DTO dto);
    FORMNO19DTO getEmployeedetailsBySelection(FORMNO19DTO dto);
  }
}
