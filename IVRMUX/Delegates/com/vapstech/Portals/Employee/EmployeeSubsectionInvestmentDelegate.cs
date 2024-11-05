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
   
public class EmployeeSubsectionInvestmentDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<EmployeeInvestmentSubsectionDTO, EmployeeInvestmentSubsectionDTO> COMMM = new CommonDelegate<EmployeeInvestmentSubsectionDTO, EmployeeInvestmentSubsectionDTO>();

        public EmployeeInvestmentSubsectionDTO onloadgetdetails(EmployeeInvestmentSubsectionDTO dto)
        {
            return COMMM.POSTPORTALData(dto, "HRInvestmentFacade/onloadgetdetails");
        }

        public EmployeeInvestmentSubsectionDTO savedetails(EmployeeInvestmentSubsectionDTO maspage)
        {
            return COMMM.POSTPORTALData(maspage, "HRInvestmentFacade/");
        }
        public EmployeeInvestmentSubsectionDTO getRecorddetailsById(int id)
        {
            return COMMM.GETPORTALData(id, "HRInvestmentFacade/getRecordById/");
        }
        public EmployeeInvestmentSubsectionDTO deleterec(EmployeeInvestmentSubsectionDTO maspage)
        { 
            return COMMM.POSTPORTALData(maspage, "HRInvestmentFacade/deactivateRecordById/");
        }
        public EmployeeInvestmentSubsectionDTO getDetailsByEmployee(EmployeeInvestmentSubsectionDTO maspage)
        {
            return COMMM.POSTPORTALData(maspage, "HRInvestmentFacade/getDetailsByEmployee/");
        }

    }
}
