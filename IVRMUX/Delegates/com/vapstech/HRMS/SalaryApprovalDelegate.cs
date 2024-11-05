using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class SalaryApprovalDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Employee_SalaryDTO, HR_Employee_SalaryDTO> COMMM = new CommonDelegate<HR_Employee_SalaryDTO, HR_Employee_SalaryDTO>();

        public HR_Employee_SalaryDTO onloadgetdetails(HR_Employee_SalaryDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "SalaryApprovalFacade/getalldetails");
        }


        //getEmployeedetailsBySelection  

        public HR_Employee_SalaryDTO getEmployeedetailsBySelection(HR_Employee_SalaryDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "SalaryApprovalFacade/getEmployeedetailsBySelection/");
        }
    }
}
