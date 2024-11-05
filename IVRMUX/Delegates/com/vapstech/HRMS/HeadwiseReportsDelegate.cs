using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class HeadwiseReportsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HeaderwiseReportDTO, HeaderwiseReportDTO> COMMM = new CommonDelegate<HeaderwiseReportDTO, HeaderwiseReportDTO>();

        public HeaderwiseReportDTO onloadgetdetails(HeaderwiseReportDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "HeadwiseReportsFacade/onloadgetdetails");
        }


        //getEmployeedetailsBySelection  

        public HeaderwiseReportDTO getEmployeedetailsBySelection(HeaderwiseReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HeadwiseReportsFacade/getEmployeedetailsBySelection/");
        }
        public HeaderwiseReportDTO get_depts(HeaderwiseReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HeadwiseReportsFacade/get_depts/");
        }
        public HeaderwiseReportDTO get_desig(HeaderwiseReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "HeadwiseReportsFacade/get_desig/");
        }


    }
}
