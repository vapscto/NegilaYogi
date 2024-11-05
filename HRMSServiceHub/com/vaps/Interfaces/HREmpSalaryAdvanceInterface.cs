using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
   public interface HREmpSalaryAdvanceInterface
    {
    HR_Emp_SalaryAdvanceDTO getBasicData(HR_Emp_SalaryAdvanceDTO dto);
    HR_Emp_SalaryAdvanceDTO SaveUpdate(HR_Emp_SalaryAdvanceDTO dto);
    HR_Emp_SalaryAdvanceDTO editData(int id);

    HR_Emp_SalaryAdvanceDTO deactivate(HR_Emp_SalaryAdvanceDTO dto);

    HR_Emp_SalaryAdvanceDTO getDetailsByEmployee(HR_Emp_SalaryAdvanceDTO dto);

        HR_Emp_SalaryAdvanceDTO searchfilter(HR_Emp_SalaryAdvanceDTO dto);

        }
}
