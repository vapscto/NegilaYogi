using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Principal
{
    public class SalaryDetailsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<SalaryDetailsDTO, SalaryDetailsDTO> COMMM = new CommonDelegate<SalaryDetailsDTO, SalaryDetailsDTO>();
       

        public SalaryDetailsDTO onloadgetdetails(SalaryDetailsDTO dto)
        {
            return COMMM.POSTPORTALData(dto, "SalaryDetailsFacade/onloadgetdetails");
        }
        public SalaryDetailsDTO Getdepartment(SalaryDetailsDTO maspage)
        {
            return COMMM.POSTPORTALData(maspage, "SalaryDetailsFacade/Getdepartment/");
        }
        public SalaryDetailsDTO get_designation(SalaryDetailsDTO maspage)
        {
            return COMMM.POSTPORTALData(maspage, "SalaryDetailsFacade/get_designation/");
        }
        public SalaryDetailsDTO get_department(SalaryDetailsDTO maspage)
        {
            return COMMM.POSTPORTALData(maspage, "SalaryDetailsFacade/get_department/");
        }

        public SalaryDetailsDTO get_employee(SalaryDetailsDTO maspage)
        {
            return COMMM.POSTPORTALData(maspage, "SalaryDetailsFacade/get_employee/");
        }
        //GetEmployeeDetailsByLeaveYearAndMonth
        public SalaryDetailsDTO GetEmployeeDetailsByLeaveYearAndMonth(SalaryDetailsDTO maspage)
        {
            return COMMM.POSTPORTALData(maspage, "SalaryDetailsFacade/GetEmployeeDetailsByLeaveYearAndMonth/");
        }

        //getEmployeedetailsBySelection  

        public SalaryDetailsDTO GenerateEmployeeSalarySlip(SalaryDetailsDTO maspage)
        {
            return COMMM.POSTPORTALData(maspage, "SalaryDetailsFacade/GenerateEmployeeSalarySlip/");
        }
    }
}
