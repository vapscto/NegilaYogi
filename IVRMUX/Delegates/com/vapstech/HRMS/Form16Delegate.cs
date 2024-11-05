using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class Form16Delegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Form16DTO, Form16DTO> COMMM = new CommonDelegate<Form16DTO, Form16DTO>();

        public Form16DTO onloadgetdetails(Form16DTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "Form16Facade/onloadgetdetails");
        }

        //GetEmployeeDetailsByLeaveYearAndMonth
        public Form16DTO GetEmployeeDetailsByLeaveYearAndMonth(Form16DTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "Form16Facade/GetEmployeeDetailsByLeaveYearAndMonth/");
        }

        //getEmployeedetailsBySelection  

        public Form16DTO GenerateEmployeeSalarySlip(Form16DTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "Form16Facade/GenerateEmployeeSalarySlip/");
        }

        //
        //public HR_Employee_SalaryDTO SendEmailSMS(HR_Employee_SalaryDTO maspage)
        //    {
        //    return COMMM.POSTDataHRMS(maspage, "EmployeeSalarySlipGenerationFacade/SendEmailSMS/");
        //    }


        }
}
