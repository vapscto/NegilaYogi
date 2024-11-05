using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class EmployeeDetailsReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<EmployeeReportsDTO, EmployeeReportsDTO> COMMM = new CommonDelegate<EmployeeReportsDTO, EmployeeReportsDTO>();

        public EmployeeReportsDTO onloadgetdetails(EmployeeReportsDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmployeeDetailsReportFacade/onloadgetdetails");
        }


        //getEmployeedetailsBySelection  

        public EmployeeReportsDTO getEmployeedetailsBySelection(EmployeeReportsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeDetailsReportFacade/getEmployeedetailsBySelection/");
        }

        public EmployeeReportsDTO FilterEmployeeData(EmployeeReportsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeDetailsReportFacade/FilterEmployeeData/");
        }

        public EmployeeReportsDTO get_depts(EmployeeReportsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeDetailsReportFacade/get_depts/");
        }
        public EmployeeReportsDTO get_desig(EmployeeReportsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeDetailsReportFacade/get_desig/");
        }
    }
}
