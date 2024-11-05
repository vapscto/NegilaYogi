using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class EmployeeSalaryDetailsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Employee_EarningsDeductionsDTO, HR_Employee_EarningsDeductionsDTO> COMMM = new CommonDelegate<HR_Employee_EarningsDeductionsDTO, HR_Employee_EarningsDeductionsDTO>();

       // CommonDelegate<MasterEmployeeDTO, HR_Employee_EarningsDeductionsDTO> COMMM2 = new CommonDelegate<MasterEmployeeDTO, HR_Employee_EarningsDeductionsDTO>();



        public HR_Employee_EarningsDeductionsDTO onloadgetdetails(HR_Employee_EarningsDeductionsDTO dto)
            {
            return COMMM.POSTDataHRMS(dto, "EmployeeSalaryDetailsFacade/onloadgetdetails");
            }

        public HR_Employee_EarningsDeductionsDTO savedetails(HR_Employee_EarningsDeductionsDTO maspage)
            {
            return COMMM.POSTDataHRMS(maspage, "EmployeeSalaryDetailsFacade/");
            }
       

        public HR_Employee_EarningsDeductionsDTO getEmployeeSalaryDetails(HR_Employee_EarningsDeductionsDTO maspage)
            {
            return COMMM.POSTDataHRMS(maspage, "EmployeeSalaryDetailsFacade/getEmployeeSalaryDetails/");
            }

        //getEmployeeSalaryDetailsByHead

        public HR_Employee_EarningsDeductionsDTO getEmployeeSalaryDetailsByHead(HR_Employee_EarningsDeductionsDTO maspage)
            {
            return COMMM.POSTDataaHRMS(maspage, "EmployeeSalaryDetailsFacade/getEmployeeSalaryDetailsByHead/");
            }

        public HR_Employee_EarningsDeductionsDTO GetEmployeeDetailsBySelected(HR_Employee_EarningsDeductionsDTO maspage)
            {
            return COMMM.POSTDataaHRMS(maspage, "EmployeeSalaryDetailsFacade/GetEmployeeDetailsBySelected/");
            }

        public HR_Employee_EarningsDeductionsDTO get_depts(HR_Employee_EarningsDeductionsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeSalaryDetailsFacade/get_depts/");
        }
        public HR_Employee_EarningsDeductionsDTO get_desig(HR_Employee_EarningsDeductionsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeSalaryDetailsFacade/get_desig/");
        }
    }
}
