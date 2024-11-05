using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Employee.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.Portals.Employee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class HREmployeeInvestmentSubSectionFacadeController : Controller
    {
        // GET: api/values
        public HREmpInvestmentSubsectionInterface _ads;

        public HREmployeeInvestmentSubSectionFacadeController(HREmpInvestmentSubsectionInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public EmployeeInvestmentSubsectionDTO getinitialdata([FromBody]EmployeeInvestmentSubsectionDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public EmployeeInvestmentSubsectionDTO Post([FromBody]EmployeeInvestmentSubsectionDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]

        public EmployeeInvestmentSubsectionDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public EmployeeInvestmentSubsectionDTO deactivateRecordById([FromBody]EmployeeInvestmentSubsectionDTO dto)
        {
            return _ads.deactivate(dto);
        }
        [Route("getDetailsByEmployee")]
        public EmployeeInvestmentSubsectionDTO getDetailsByEmployee([FromBody]EmployeeInvestmentSubsectionDTO dto)
        {
            return _ads.getDetailsByEmployee(dto);
        }

    }
}
