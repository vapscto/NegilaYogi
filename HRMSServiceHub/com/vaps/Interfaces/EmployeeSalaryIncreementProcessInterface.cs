using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface EmployeeSalaryIncreementProcessInterface
    {
        EmployeeSalaryIncreementProcessDTO getBasicData(EmployeeSalaryIncreementProcessDTO dto);
        EmployeeSalaryIncreementProcessDTO getReport(EmployeeSalaryIncreementProcessDTO dto);
        EmployeeSalaryIncreementProcessDTO Empdetails(EmployeeSalaryIncreementProcessDTO dto);
        EmployeeSalaryIncreementProcessDTO SaveUpdate(EmployeeSalaryIncreementProcessDTO dto);
        EmployeeSalaryIncreementProcessDTO editData(int id);
        EmployeeSalaryIncreementProcessDTO deactivate(EmployeeSalaryIncreementProcessDTO dto);

    }
}
