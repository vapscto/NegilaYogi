using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class EmployeeOfferAndExperienceReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<EmployeeOfferAndExperienceReportDTO, EmployeeOfferAndExperienceReportDTO> COMMM = new CommonDelegate<EmployeeOfferAndExperienceReportDTO, EmployeeOfferAndExperienceReportDTO>();

        public EmployeeOfferAndExperienceReportDTO onloadgetdetails(EmployeeOfferAndExperienceReportDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmployeeOfferAndExperienceReportFacade/onloadgetdetails");
        }

        public EmployeeOfferAndExperienceReportDTO FilterEmployeeData(EmployeeOfferAndExperienceReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeOfferAndExperienceReportFacade/FilterEmployeeData/");
        }
        //getEmployeedetailsBySelection  

        public EmployeeOfferAndExperienceReportDTO getEmployeedetailsBySelection(EmployeeOfferAndExperienceReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeOfferAndExperienceReportFacade/getEmployeedetailsBySelection/");
        }
    }
}
