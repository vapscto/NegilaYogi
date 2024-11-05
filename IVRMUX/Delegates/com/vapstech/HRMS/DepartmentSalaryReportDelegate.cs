using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class DepartmentSalaryReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Department_SalaryDTO, HR_Department_SalaryDTO> COMMM = new CommonDelegate<HR_Department_SalaryDTO, HR_Department_SalaryDTO>();

        public HR_Department_SalaryDTO onloadgetdetails(HR_Department_SalaryDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "DepartmentSalaryReportFacade/onloadgetdetails");
        }


        //getEmployeedetailsBySelection  

        public HR_Department_SalaryDTO getEmployeedetailsBySelection(HR_Department_SalaryDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "DepartmentSalaryReportFacade/getEmployeedetailsBySelection/");
        }

        public HR_Department_SalaryDTO get_depts(HR_Department_SalaryDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "DepartmentSalaryReportFacade/get_depts/");
        }
        public HR_Department_SalaryDTO get_desig(HR_Department_SalaryDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "DepartmentSalaryReportFacade/get_desig/");
        }
    }
}

   