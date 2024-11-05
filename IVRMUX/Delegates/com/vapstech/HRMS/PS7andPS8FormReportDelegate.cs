using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class PS7andPS8FormReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<PFReportsDTO, PFReportsDTO> COMMM = new CommonDelegate<PFReportsDTO, PFReportsDTO>();

        public PFReportsDTO onloadgetdetails(PFReportsDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "PS7andPS8FormReportFacade/onloadgetdetails");
        }

        //getEmployeedetailsBySelection  ps7
        public PFReportsDTO getEmployeedetailsBySelection(PFReportsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "PS7andPS8FormReportFacade/getEmployeedetailsBySelection/");
        }

        //getEmployeedetailsBySelection  ps8
        public PFReportsDTO getdataps8(PFReportsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "PS7andPS8FormReportFacade/getdataps8/");
        }

    }
}
