using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
   public interface HREmpTDSQuarterInterface
    {

        HR_Emp_TDS_QUARTERDTO getBasicData(HR_Emp_TDS_QUARTERDTO dto);
        HR_Emp_TDS_QUARTERDTO SaveUpdate(HR_Emp_TDS_QUARTERDTO dto);
        HR_Emp_TDS_QUARTERDTO editData(int id);
        HR_Emp_TDS_QUARTERDTO deactivate(HR_Emp_TDS_QUARTERDTO dto);
        HR_Emp_TDS_QUARTERDTO getDetailsByEmployee(HR_Emp_TDS_QUARTERDTO dto);
        HR_Emp_TDS_QUARTERDTO getquarter(HR_Emp_TDS_QUARTERDTO dto);

    }
}
