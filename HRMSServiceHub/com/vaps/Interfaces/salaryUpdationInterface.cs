using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
  public interface salaryUpdationInterface
    {
        salaryUpdationDTO getBasicData(salaryUpdationDTO dto);

        //FilterEmployeeData

        salaryUpdationDTO FilterEmployeeData(salaryUpdationDTO dto);
        salaryUpdationDTO getEmployeedetailsBySelection(salaryUpdationDTO dto);

        salaryUpdationDTO get_depts(salaryUpdationDTO dto);

        salaryUpdationDTO get_desig(salaryUpdationDTO dto);
    }
}
