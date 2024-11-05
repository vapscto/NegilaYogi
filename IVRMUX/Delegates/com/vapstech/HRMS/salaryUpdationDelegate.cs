using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class salaryUpdationDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<salaryUpdationDTO, salaryUpdationDTO> COMMM = new CommonDelegate<salaryUpdationDTO, salaryUpdationDTO>();

        public salaryUpdationDTO onloadgetdetails(salaryUpdationDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "salaryUpdationFacade/onloadgetdetails");
        }

        //FilterEmployeeData
        public salaryUpdationDTO FilterEmployeeData(salaryUpdationDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "salaryUpdationFacade/FilterEmployeeData/");
        }

        //getEmployeedetailsBySelection   

        public salaryUpdationDTO getEmployeedetailsBySelection(salaryUpdationDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "salaryUpdationFacade/getEmployeedetailsBySelection/");
        }

        public salaryUpdationDTO get_depts(salaryUpdationDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "salaryUpdationFacade/get_depts/");
        }
        public salaryUpdationDTO get_desig(salaryUpdationDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "salaryUpdationFacade/get_desig/");
        }
    }
}
