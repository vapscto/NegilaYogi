using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class EmployeeStrengthReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<EmployeeStrengthReportDTO, EmployeeStrengthReportDTO> COMMM = new CommonDelegate<EmployeeStrengthReportDTO, EmployeeStrengthReportDTO>();

        public EmployeeStrengthReportDTO onloadgetdetails(EmployeeStrengthReportDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmployeeStrengthReportFacade/onloadgetdetails");
        }


        //getEmployeedetailsBySelection  

        public EmployeeStrengthReportDTO getEmployeedetailsBySelection(EmployeeStrengthReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeStrengthReportFacade/getEmployeedetailsBySelection/");
        }

        public EmployeeStrengthReportDTO get_depts(EmployeeStrengthReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeStrengthReportFacade/get_depts/");
        }
        public EmployeeStrengthReportDTO get_desig(EmployeeStrengthReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeStrengthReportFacade/get_desig/");
        }
    }
}
