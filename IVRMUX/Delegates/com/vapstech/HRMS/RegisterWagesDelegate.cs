using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class RegisterWagesDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_EmployeeRegisterDTO, HR_EmployeeRegisterDTO> COMMM = new CommonDelegate<HR_EmployeeRegisterDTO, HR_EmployeeRegisterDTO>();

        public HR_EmployeeRegisterDTO onloadgetdetails(HR_EmployeeRegisterDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "RegisterWagesReportFacade/onloadgetdetails");
        }


        //getEmployeedetailsBySelection  

        public HR_EmployeeRegisterDTO getEmployeedetailsBySelection(HR_EmployeeRegisterDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "RegisterWagesReportFacade/getEmployeedetailsBySelection/");
        }

        public HR_EmployeeRegisterDTO get_depts(HR_EmployeeRegisterDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "RegisterWagesReportFacade/get_depts/");
        }
        public HR_EmployeeRegisterDTO get_desig(HR_EmployeeRegisterDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "RegisterWagesReportFacade/get_desig/");
        }
    }
}

   