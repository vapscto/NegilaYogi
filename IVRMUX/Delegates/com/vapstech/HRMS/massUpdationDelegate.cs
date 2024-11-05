using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class massUpdationDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<massUpdationDTO, massUpdationDTO> COMMM = new CommonDelegate<massUpdationDTO, massUpdationDTO>();

        public massUpdationDTO onloadgetdetails(massUpdationDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "massUpdationFacade/onloadgetdetails");
        }

        //FilterEmployeeData
        public massUpdationDTO FilterEmployeeData(massUpdationDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "massUpdationFacade/FilterEmployeeData/");
        }

        //getEmployeedetailsBySelection   

        public massUpdationDTO getEmployeedetailsBySelection(massUpdationDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "massUpdationFacade/getEmployeedetailsBySelection/");
        }


        public massUpdationDTO get_depts(massUpdationDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "massUpdationFacade/get_depts/");
        }
        public massUpdationDTO get_desig(massUpdationDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "massUpdationFacade/get_desig/");
        }
    }
}
