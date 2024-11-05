using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
   public interface HREmpInvestmentSubsectionInterface
    {

        EmployeeInvestmentSubsectionDTO getBasicData(EmployeeInvestmentSubsectionDTO dto);
        EmployeeInvestmentSubsectionDTO SaveUpdate(EmployeeInvestmentSubsectionDTO dto);
        EmployeeInvestmentSubsectionDTO editData(int id);
        EmployeeInvestmentSubsectionDTO deactivate(EmployeeInvestmentSubsectionDTO dto);
        EmployeeInvestmentSubsectionDTO getDetailsByEmployee(EmployeeInvestmentSubsectionDTO dto);

    }
}
