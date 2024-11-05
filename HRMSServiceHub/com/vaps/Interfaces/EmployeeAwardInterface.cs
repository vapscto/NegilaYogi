using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Interfaces
{
   public interface EmployeeAwardInterface
    {
        HR_Employee_Awards_DTO getalldetails(HR_Employee_Awards_DTO data);
        HR_Employee_Awards_DTO get_depchange(HR_Employee_Awards_DTO data);
        HR_Employee_Awards_DTO get_employee(HR_Employee_Awards_DTO data);
        HR_Employee_Awards_DTO saverecord(HR_Employee_Awards_DTO data);
        HR_Employee_Awards_DTO editrecord(HR_Employee_Awards_DTO data);
        HR_Employee_Awards_DTO deactive(HR_Employee_Awards_DTO data);

    }
}
