using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Controllers.com.vapstech.Portals.Employee
{
   
public class EmployeeInvestmentothersDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<EmployeeInvestmentothersDTO, EmployeeInvestmentothersDTO> COMMM = new CommonDelegate<EmployeeInvestmentothersDTO, EmployeeInvestmentothersDTO>();

        public EmployeeInvestmentothersDTO onloadgetdetails(EmployeeInvestmentothersDTO dto)
        {
            return COMMM.POSTPORTALData(dto, "HREmployeeInvestmentOtherFacade/onloadgetdetails");
        }

        public EmployeeInvestmentothersDTO savedetails(EmployeeInvestmentothersDTO maspage)
        {
            return COMMM.POSTPORTALData(maspage, "HREmployeeInvestmentOtherFacade/");
        }
        public EmployeeInvestmentothersDTO getRecorddetailsById(int id)
        {
            return COMMM.GETPORTALData(id, "HREmployeeInvestmentOtherFacade/getRecordById/");
        }
        public EmployeeInvestmentothersDTO deleterec(EmployeeInvestmentothersDTO maspage)
        { 
            return COMMM.POSTPORTALData(maspage, "HREmployeeInvestmentOtherFacade/deactivateRecordById/");
        }
        public EmployeeInvestmentothersDTO getDetailsByEmployee(EmployeeInvestmentothersDTO maspage)
        {
            return COMMM.POSTPORTALData(maspage, "HREmployeeInvestmentOtherFacade/getDetailsByEmployee/");
        }

    }
}
