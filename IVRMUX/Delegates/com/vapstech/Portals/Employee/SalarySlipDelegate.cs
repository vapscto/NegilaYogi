using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Employee
{
    public class SalarySlipDelegats
    {
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Employee_SalaryDTO, HR_Employee_SalaryDTO> COMMM = new CommonDelegate<HR_Employee_SalaryDTO, HR_Employee_SalaryDTO>();    
        public HR_Employee_SalaryDTO onloadgetdetails(HR_Employee_SalaryDTO dto)
        {
            return COMMM.POSTPORTALData(dto, "SalarySlipFacade/onloadgetdetails");   
        }

        //GetEmployeeDetailsByLeaveYearAndMonth
        public HR_Employee_SalaryDTO GetEmployeeDetailsByLeaveYearAndMonth(HR_Employee_SalaryDTO maspage)
        {
            return COMMM.POSTPORTALData(maspage, "SalarySlipFacade/GetEmployeeDetailsByLeaveYearAndMonth/");
        }

        //getEmployeedetailsBySelection  
          
        public HR_Employee_SalaryDTO GenerateEmployeeSalarySlip(HR_Employee_SalaryDTO maspage)
        {
            return COMMM.POSTPORTALData(maspage, "SalarySlipFacade/GenerateEmployeeSalarySlip/");
        }

      


    }
}
