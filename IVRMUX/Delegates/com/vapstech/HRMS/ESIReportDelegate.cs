using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class ESIReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ESIReportDTO, ESIReportDTO> COMMM = new CommonDelegate<ESIReportDTO, ESIReportDTO>();

        public ESIReportDTO onloadgetdetails(ESIReportDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "ESIReportFacade/onloadgetdetails");
        }


        //getEmployeedetailsBySelection  

        public ESIReportDTO getEmployeedetailsBySelection(ESIReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "ESIReportFacade/getEmployeedetailsBySelection/");
        }
        public ESIReportDTO get_depts(ESIReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "ESIReportFacade/get_depts/");
        }
        public ESIReportDTO get_desig(ESIReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "ESIReportFacade/get_desig/");
        }

    }
}
