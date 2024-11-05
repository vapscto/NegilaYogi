using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
   public interface massUpdationInterface
    {
        massUpdationDTO getBasicData(massUpdationDTO dto);

        //FilterEmployeeData

        massUpdationDTO FilterEmployeeData(massUpdationDTO dto);
        massUpdationDTO getEmployeedetailsBySelection(massUpdationDTO dto);
        massUpdationDTO get_depts(massUpdationDTO dto);

        massUpdationDTO get_desig(massUpdationDTO dto);

    }
}
