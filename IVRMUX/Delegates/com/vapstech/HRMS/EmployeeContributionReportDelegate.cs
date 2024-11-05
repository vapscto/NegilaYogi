using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class EmployeeContributionReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<EmployeeContributionReportDTO, EmployeeContributionReportDTO> COMMM = new CommonDelegate<EmployeeContributionReportDTO, EmployeeContributionReportDTO>();

        public EmployeeContributionReportDTO onloadgetdetails(EmployeeContributionReportDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmployeeContributionReportFacade/onloadgetdetails");
        }

        //FilterEmployeeData
        public EmployeeContributionReportDTO FilterEmployeeData(EmployeeContributionReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeContributionReportFacade/FilterEmployeeData/");
        }

        //getEmployeedetailsBySelection   

        public EmployeeContributionReportDTO getEmployeedetailsBySelection(EmployeeContributionReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeContributionReportFacade/getEmployeedetailsBySelection/");
        }

        public EmployeeContributionReportDTO get_depts(EmployeeContributionReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeContributionReportFacade/get_depts/");
        }
        public EmployeeContributionReportDTO get_desig(EmployeeContributionReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeContributionReportFacade/get_desig/");
        }
    }
}
