using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class CumulativeSalaryReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Employee_SalaryDTO, HR_Employee_SalaryDTO> COMMM = new CommonDelegate<HR_Employee_SalaryDTO, HR_Employee_SalaryDTO>();
        CommonDelegate<HR_Employee_SalaryDTO.HR_Cumulative_Salary_Report, HR_Employee_SalaryDTO.HR_Cumulative_Salary_Report> COMMMM = new CommonDelegate<HR_Employee_SalaryDTO.HR_Cumulative_Salary_Report, HR_Employee_SalaryDTO.HR_Cumulative_Salary_Report>();

        public HR_Employee_SalaryDTO onloadgetdetails(HR_Employee_SalaryDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "CumulativeSalaryReportFacade/onloadgetdetails");
        }


        //getEmployeedetailsBySelection  

        public HR_Employee_SalaryDTO getEmployeedetailsBySelection(HR_Employee_SalaryDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "CumulativeSalaryReportFacade/getEmployeedetailsBySelection/");
        }
        public HR_Employee_SalaryDTO.HR_Cumulative_Salary_Report getCumulativeSalaryReport(HR_Employee_SalaryDTO.HR_Cumulative_Salary_Report maspage)
        {
            return COMMMM.POSTDataHRMS(maspage, "CumulativeSalaryReportFacade/getCumulativeSalaryReport/");
        }

        public HR_Employee_SalaryDTO getEmployeedetailsByDepartment(HR_Employee_SalaryDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "CumulativeSalaryReportFacade/getEmployeedetailsByDepartment/");
        }

        public HR_Employee_SalaryDTO get_depts(HR_Employee_SalaryDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "CumulativeSalaryReportFacade/get_depts/");
        }
        public HR_Employee_SalaryDTO get_desig(HR_Employee_SalaryDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "CumulativeSalaryReportFacade/get_desig/");
        }

        public HR_Employee_SalaryDTO.HR_Cumulative_Salary_Report yearlyreport(HR_Employee_SalaryDTO.HR_Cumulative_Salary_Report maspage)
        {
            return COMMMM.POSTDataHRMS(maspage, "CumulativeSalaryReportFacade/yearlyreport/");
        }

        public HR_Employee_SalaryDTO.HR_Cumulative_Salary_Report headwisereport(HR_Employee_SalaryDTO.HR_Cumulative_Salary_Report maspage)
        {
            return COMMMM.POSTDataHRMS(maspage, "CumulativeSalaryReportFacade/headwisereport/");
        }

    }
}

   