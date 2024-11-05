using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class EmployeeYearlyReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<EmployeeYearlyReportDTO, EmployeeYearlyReportDTO> COMMM = new CommonDelegate<EmployeeYearlyReportDTO, EmployeeYearlyReportDTO>();

        public EmployeeYearlyReportDTO onloadgetdetails(EmployeeYearlyReportDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmployeeYearlyFacade/onloadgetdetails");
        }
        public EmployeeYearlyReportDTO filterEmployeedetailsBySelection(EmployeeYearlyReportDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmployeeYearlyFacade/filterEmployeedetailsBySelection");
        }

        //getEmployeedetailsBySelection  

        public EmployeeYearlyReportDTO getEmployeedetailsBySelection(EmployeeYearlyReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeYearlyFacade/getEmployeedetailsBySelection/");
        }

        public EmployeeYearlyReportDTO get_depts(EmployeeYearlyReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeYearlyFacade/get_depts/");
        }
        public EmployeeYearlyReportDTO get_desig(EmployeeYearlyReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeYearlyFacade/get_desig/");
        }

        public EmployeeYearlyReportDTO reportBetweenDatesBySelection(EmployeeYearlyReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeYearlyFacade/reportBetweenDatesBySelection/");
        }
    }
}
