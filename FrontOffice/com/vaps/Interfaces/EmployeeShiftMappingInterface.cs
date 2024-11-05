using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Interfaces
{
  public  interface EmployeeShiftMappingInterface
    {
        EmployeeShiftMappingDTO savedetail(EmployeeShiftMappingDTO objcategory);
        EmployeeShiftMappingDTO getdetails(int id);
        EmployeeShiftMappingDTO Shiftname(EmployeeShiftMappingDTO data);
        EmployeeShiftMappingDTO editdetails(int id);
        EmployeeShiftMappingDTO deleterec(EmployeeShiftMappingDTO data);
        EmployeeShiftMappingDTO get_departments(EmployeeShiftMappingDTO data);
        EmployeeShiftMappingDTO get_designation(EmployeeShiftMappingDTO data);
        EmployeeShiftMappingDTO get_employee(EmployeeShiftMappingDTO data);
    }
}
