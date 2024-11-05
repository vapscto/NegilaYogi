using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementServiceHub.com.vaps.Interfaces
{
   public interface EmployeeOnDutyReportInterface
    {
        EmployeeOnDutyReportDTO getalldetails(EmployeeOnDutyReportDTO data);
        EmployeeOnDutyReportDTO getEmployeedetailsBySelection(EmployeeOnDutyReportDTO dto);
    }
}
