using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
   public interface FORM12Interface
    {
    FORM12DTO getBasicData(FORM12DTO dto);

    //FilterEmployeeData

    FORM12DTO FilterEmployeeData(FORM12DTO dto);
    FORM12DTO getEmployeedetailsBySelection(FORM12DTO dto);
  }
}
