using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
   public interface HREmpInvestmentInterface
    {

        EmployeeInvestmentDTO getBasicData(EmployeeInvestmentDTO dto);
        EmployeeInvestmentDTO SaveUpdate(EmployeeInvestmentDTO dto);
        EmployeeInvestmentDTO editData(int id);
        EmployeeInvestmentDTO deactivate(EmployeeInvestmentDTO dto);
        EmployeeInvestmentDTO getDetailsByEmployee(EmployeeInvestmentDTO dto);

    }
}
