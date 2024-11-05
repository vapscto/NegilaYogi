using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class EmployeeSalarySlipGenerationDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Employee_SalaryDTO, HR_Employee_SalaryDTO> COMMM = new CommonDelegate<HR_Employee_SalaryDTO, HR_Employee_SalaryDTO>();

        public HR_Employee_SalaryDTO onloadgetdetails(HR_Employee_SalaryDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmployeeSalarySlipGenerationFacade/onloadgetdetails");
        }

        //GetEmployeeDetailsByLeaveYearAndMonth
        public HR_Employee_SalaryDTO GetEmployeeDetailsByLeaveYearAndMonth(HR_Employee_SalaryDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeSalarySlipGenerationFacade/GetEmployeeDetailsByLeaveYearAndMonth/");
        }

        //getEmployeedetailsBySelection  

        public HR_Employee_SalaryDTO GenerateEmployeeSalarySlip(HR_Employee_SalaryDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeSalarySlipGenerationFacade/GenerateEmployeeSalarySlip/");
        }

        //
        public HR_Employee_SalaryDTO SendEmailSMS(HR_Employee_SalaryDTO maspage)
            {
            return COMMM.POSTDataHRMS(maspage, "EmployeeSalarySlipGenerationFacade/SendEmailSMS/");
            }
        public HR_Employee_SalaryDTO get_depts(HR_Employee_SalaryDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeSalarySlipGenerationFacade/get_depts/");
        }
        public HR_Employee_SalaryDTO get_desig(HR_Employee_SalaryDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeSalarySlipGenerationFacade/get_desig/");
        }

    }
}
