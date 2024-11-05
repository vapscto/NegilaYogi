using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Interfaces
{
    public interface EmployeeDataImportInterface
    {
        EmployeeDataImportDTO Savedata(EmployeeDataImportDTO data);
        EmployeeDataImportDTO getdetails(int id);
        EmployeeDataImportDTO deactiveY(EmployeeDataImportDTO data);
    }
}
