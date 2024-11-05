using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class ArrearSalaryReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Arrear_SalaryDTO, HR_Arrear_SalaryDTO> COMMM = new CommonDelegate<HR_Arrear_SalaryDTO, HR_Arrear_SalaryDTO>();

        public HR_Arrear_SalaryDTO onloadgetdetails(HR_Arrear_SalaryDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "ArrearSalaryFacade/onloadgetdetails");
        }


        //getEmployeedetailsBySelection  

        public HR_Arrear_SalaryDTO getEmployeedetailsBySelection(HR_Arrear_SalaryDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "ArrearSalaryFacade/getEmployeedetailsBySelection/");
        }

        public HR_Arrear_SalaryDTO get_depts(HR_Arrear_SalaryDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "ArrearSalaryFacade/get_depts/");
        }
        public HR_Arrear_SalaryDTO get_desig(HR_Arrear_SalaryDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "ArrearSalaryFacade/get_desig/");
        }
    }
}

   