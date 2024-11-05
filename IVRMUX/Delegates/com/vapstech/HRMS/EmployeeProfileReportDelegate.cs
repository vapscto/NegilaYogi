using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class EmployeeProfileReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<EmployeeProfileReportDTO, EmployeeProfileReportDTO> COMMM = new CommonDelegate<EmployeeProfileReportDTO, EmployeeProfileReportDTO>();

        public EmployeeProfileReportDTO onloadgetdetails(EmployeeProfileReportDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmployeeProfileReportFacade/onloadgetdetails");
        }
        public EmployeeProfileReportDTO filterEmployeedetailsBySelection(EmployeeProfileReportDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmployeeProfileReportFacade/filterEmployeedetailsBySelection");
        }

        //getEmployeedetailsBySelection  

        public EmployeeProfileReportDTO getEmployeedetailsBySelection(EmployeeProfileReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeProfileReportFacade/getEmployeedetailsBySelection/");
        }

        public EmployeeProfileReportDTO get_depts(EmployeeProfileReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeProfileReportFacade/get_depts/");
        }
        public EmployeeProfileReportDTO get_desig(EmployeeProfileReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeProfileReportFacade/get_desig/");
        }
    }
}
