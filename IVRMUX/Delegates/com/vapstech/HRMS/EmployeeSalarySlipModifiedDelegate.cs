using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;


namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class EmployeeSalarySlipModifiedDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Employee_SalaryModifiedDTO, HR_Employee_SalaryModifiedDTO> COMMM = new CommonDelegate<HR_Employee_SalaryModifiedDTO, HR_Employee_SalaryModifiedDTO>();

        public HR_Employee_SalaryModifiedDTO onloadgetdetails(HR_Employee_SalaryModifiedDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmployeeSalarySlipFacadeModified/onloadgetdetails");
        }

        //GetEmployeeDetailsByLeaveYearAndMonth
        public HR_Employee_SalaryModifiedDTO GetEmployeeDetailsByLeaveYearAndMonth(HR_Employee_SalaryModifiedDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeSalarySlipFacadeModified/GetEmployeeDetailsByLeaveYearAndMonth/");
        }

        //getEmployeedetailsBySelection  

        public HR_Employee_SalaryModifiedDTO GenerateEmployeeSalarySlip(HR_Employee_SalaryModifiedDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeSalarySlipFacadeModified/GenerateEmployeeSalarySlip/");
        }

        
        public HR_Employee_SalaryModifiedDTO SendEmailSMS(HR_Employee_SalaryModifiedDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeSalarySlipFacadeModified/SendEmailSMS/");
        }
        public HR_Employee_SalaryModifiedDTO get_depts(HR_Employee_SalaryModifiedDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeSalarySlipFacadeModified/get_depts/");
        }

        //public HR_Employee_SalaryModifiedDTO get_Months(HR_Employee_SalaryModifiedDTO maspage)
        //{
        //    return COMMM.POSTDataHRMS(maspage, "EmployeeSalarySlipGenerationFacade/get_Months/");
        //}

        public HR_Employee_SalaryModifiedDTO get_desig(HR_Employee_SalaryModifiedDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeSalarySlipFacadeModified/get_desig/");
        }
    }
}
