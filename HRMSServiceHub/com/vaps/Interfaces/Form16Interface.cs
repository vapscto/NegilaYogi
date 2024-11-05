using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface Form16Interface
    {
        Form16DTO getBasicData(Form16DTO dto);
        Form16DTO GetEmployeeDetailsByLeaveYearAndMonth(Form16DTO dto);
        Task<Form16DTO> GenerateEmployeeSalarySlip(Form16DTO dto);
        //Task<HR_Employee_SalaryDTO> SendEmailSMS(HR_Employee_SalaryDTO dto);


        }
}
