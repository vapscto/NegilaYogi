using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
   public interface HREmpInvestmentotherInterface
    {

        EmployeeInvestmentothersDTO getBasicData(EmployeeInvestmentothersDTO dto);
        EmployeeInvestmentothersDTO SaveUpdate(EmployeeInvestmentothersDTO dto);
        EmployeeInvestmentothersDTO editData(int id);
        EmployeeInvestmentothersDTO deactivate(EmployeeInvestmentothersDTO dto);
        EmployeeInvestmentothersDTO getDetailsByEmployee(EmployeeInvestmentothersDTO dto);

    }
}
