using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class EPFcontributionRegisterDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<PFReportsDTO, PFReportsDTO> COMMM = new CommonDelegate<PFReportsDTO, PFReportsDTO>();

        public PFReportsDTO onloadgetdetails(PFReportsDTO dto)
        {
          return COMMM.POSTDataHRMS(dto, "EPFcontributionRegisterFacade/onloadgetdetails");
        }


        //getEmployeedetailsBySelection  

        public PFReportsDTO getEmployeedetailsBySelection(PFReportsDTO maspage)
        {
          return COMMM.POSTDataHRMS(maspage, "EPFcontributionRegisterFacade/getEmployeedetailsBySelection/");
        }

        public PFReportsDTO getEmployeedetailsBySelectionBBKV(PFReportsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EPFcontributionRegisterFacade/getEmployeedetailsBySelectionBBKV/");
        }

        public PFReportsDTO FilterEmployeeData(PFReportsDTO maspage)
        {
          return COMMM.POSTDataHRMS(maspage, "EPFcontributionRegisterFacade/FilterEmployeeData/");
        }

        public PFReportsDTO get_depts(PFReportsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EPFcontributionRegisterFacade/get_depts/");
        }
        public PFReportsDTO get_desig(PFReportsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EPFcontributionRegisterFacade/get_desig/");
        }

        public PFReportsDTO getEmployeedetailsBySelectionStJames(PFReportsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EPFcontributionRegisterFacade/getEmployeedetailsBySelectionStJames/");
        }
    }
}
