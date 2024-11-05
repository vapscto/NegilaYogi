using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Chirman;


namespace PortalHub.com.vaps.Chairman.Interfaces
{
    public interface Ch_EmployeeSalaryDetailsInterface
    {
        Emp_salaryDTO getdata(Emp_salaryDTO obj);

      
        Emp_salaryDTO onmonth(Emp_salaryDTO data);
        
    }
}
