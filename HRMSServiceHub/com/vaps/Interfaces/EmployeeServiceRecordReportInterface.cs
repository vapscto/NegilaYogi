using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface EmployeeServiceRecordReportInterface
    {
        EmployeeServiceRecordReportDTO getBasicData(EmployeeServiceRecordReportDTO dto);

        EmployeeServiceRecordReportDTO FilterEmployeeData(EmployeeServiceRecordReportDTO dto);
        Task<EmployeeServiceRecordReportDTO> getEmployeedetailsBySelection(EmployeeServiceRecordReportDTO dto);

        EmployeeServiceRecordReportDTO get_depts(EmployeeServiceRecordReportDTO dto);

        EmployeeServiceRecordReportDTO get_desig(EmployeeServiceRecordReportDTO dto);



    }
}
