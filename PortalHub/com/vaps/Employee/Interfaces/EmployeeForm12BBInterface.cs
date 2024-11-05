

using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
    public interface EmployeeForm12BBInterface
    {
        Employee12BBDTO getdata(Employee12BBDTO obj);

        Employee12BBDTO getsalaryalldetails(Employee12BBDTO obj);
    }
}

