using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
   public interface HREmpAllowanceInterface
    {

        HR_Emp_AllowanceDTO getBasicData(HR_Emp_AllowanceDTO dto);
        HR_Emp_AllowanceDTO SaveUpdate(HR_Emp_AllowanceDTO dto);
        HR_Emp_AllowanceDTO editData(int id);
        HR_Emp_AllowanceDTO deactivate(HR_Emp_AllowanceDTO dto);
        HR_Emp_AllowanceDTO getDetailsByEmployee(HR_Emp_AllowanceDTO dto);

    }
}
