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
   
public class EmployeeInvestmentDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<EmployeeInvestmentDTO, EmployeeInvestmentDTO> COMMM = new CommonDelegate<EmployeeInvestmentDTO, EmployeeInvestmentDTO>();

        public EmployeeInvestmentDTO onloadgetdetails(EmployeeInvestmentDTO dto)
        {
            return COMMM.POSTPORTALData(dto, "HRInvestmentFacade/onloadgetdetails");
        }

        public EmployeeInvestmentDTO savedetails(EmployeeInvestmentDTO maspage)
        {
            return COMMM.POSTPORTALData(maspage, "HRInvestmentFacade/");
        }
        public EmployeeInvestmentDTO getRecorddetailsById(int id)
        {
            return COMMM.GETPORTALData(id, "HRInvestmentFacade/getRecordById/");
        }
        public EmployeeInvestmentDTO deleterec(EmployeeInvestmentDTO maspage)
        { 
            return COMMM.POSTPORTALData(maspage, "HRInvestmentFacade/deactivateRecordById/");
        }
        public EmployeeInvestmentDTO getDetailsByEmployee(EmployeeInvestmentDTO maspage)
        {
            return COMMM.POSTPORTALData(maspage, "HRInvestmentFacade/getDetailsByEmployee/");
        }

    }
}
