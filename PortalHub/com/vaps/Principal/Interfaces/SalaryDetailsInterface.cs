using PreadmissionDTOs.com.vaps.Portals.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Principal.Interfaces
{
    public interface SalaryDetailsInterface
    {
        SalaryDetailsDTO getBasicData(SalaryDetailsDTO dto);
        SalaryDetailsDTO GetEmployeeDetailsByLeaveYearAndMonth(SalaryDetailsDTO dto);
        Task<SalaryDetailsDTO> GenerateEmployeeSalarySlip(SalaryDetailsDTO dto);
        SalaryDetailsDTO Getdepartment(SalaryDetailsDTO dto);
        SalaryDetailsDTO get_designation(SalaryDetailsDTO dto);
        SalaryDetailsDTO get_department(SalaryDetailsDTO dto);
        SalaryDetailsDTO get_employee(SalaryDetailsDTO dto);
    }
}
