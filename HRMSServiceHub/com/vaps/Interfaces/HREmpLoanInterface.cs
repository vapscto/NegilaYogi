using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
   public interface HREmpLoanInterface
    {

    HR_Emp_LoanDTO getBasicData(HR_Emp_LoanDTO dto);
    HR_Emp_LoanDTO SaveUpdate(HR_Emp_LoanDTO dto);
    HR_Emp_LoanDTO editData(int id);
    HR_Emp_LoanDTO deactivate(HR_Emp_LoanDTO dto);
    HR_Emp_LoanDTO getDetailsByEmployee(HR_Emp_LoanDTO dto);

    }
}
