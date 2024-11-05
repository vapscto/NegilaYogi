using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
   public interface HREmpOtherIncomeInterface
    {

        HR_Emp_otherIncomeDTO getBasicData(HR_Emp_otherIncomeDTO dto);
        HR_Emp_otherIncomeDTO SaveUpdate(HR_Emp_otherIncomeDTO dto);
        HR_Emp_otherIncomeDTO editData(int id);
        HR_Emp_otherIncomeDTO deactivate(HR_Emp_otherIncomeDTO dto);
        HR_Emp_otherIncomeDTO getDetailsByEmployee(HR_Emp_otherIncomeDTO dto);

    }
}
