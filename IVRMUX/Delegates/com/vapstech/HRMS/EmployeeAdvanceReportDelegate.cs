using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class EmployeeAdvanceReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        //CommonDelegate<HR_Emp_SalaryAdvanceDTO, HR_Emp_SalaryAdvanceDTO> COMMM = new CommonDelegate<HR_Emp_SalaryAdvanceDTO, HR_Emp_SalaryAdvanceDTO>();
        CommonDelegate<AdvanceReportDTO, AdvanceReportDTO> COMMM1 = new CommonDelegate<AdvanceReportDTO, AdvanceReportDTO>();

        public AdvanceReportDTO onloadgetdetails(AdvanceReportDTO dto)
        {
            return COMMM1.POSTDataaHRMS(dto, "SalaryAdvanceReportFacade/onloadgetdetails");
        }

        //getEmployeedetailsBySelection  

        public AdvanceReportDTO getEmployeedetailsBySelection(AdvanceReportDTO maspage)
        {
            return COMMM1.POSTDataHRMS(maspage, "SalaryAdvanceReportFacade/getEmployeedetailsBySelection/");
        }

        public AdvanceReportDTO get_depts(AdvanceReportDTO maspage)
        {
            return COMMM1.POSTDataHRMS(maspage, "SalaryAdvanceReportFacade/get_depts/");
        }
        public AdvanceReportDTO get_desig(AdvanceReportDTO maspage)
        {
            return COMMM1.POSTDataHRMS(maspage, "SalaryAdvanceReportFacade/get_desig/");
        }
    }
}

