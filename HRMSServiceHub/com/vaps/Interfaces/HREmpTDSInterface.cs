using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
   public interface HREmpTDSInterface
    {

        HR_Emp_TDSDTO getBasicData(HR_Emp_TDSDTO dto);
        HR_Emp_TDSDTO SaveUpdate(HR_Emp_TDSDTO dto);
        HR_Emp_TDSDTO editData(int id);
        HR_Emp_TDSDTO deactivate(HR_Emp_TDSDTO dto);
        HR_Emp_TDSDTO getDetailsByEmployee(HR_Emp_TDSDTO dto);

    }
}
