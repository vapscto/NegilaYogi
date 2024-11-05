using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementServiceHub.com.vaps.Interfaces
{
  public  interface PeriodWseLeavReportInterface
    {
        LeaveCreditDTO getdata(LeaveCreditDTO data);
        LeaveCreditDTO get_departments(LeaveCreditDTO data);
        LeaveCreditDTO get_designation(LeaveCreditDTO data);
        LeaveCreditDTO get_employee(LeaveCreditDTO data);
        LeaveCreditDTO getreport(LeaveCreditDTO data);
        LeaveCreditDTO getsiglerpt(LeaveCreditDTO data);
    }
}
