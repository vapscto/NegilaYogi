using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using PortalHub.com.vaps.Principal.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Principal.Controllers
{
    [Route("api/[controller]")]
    public class SalaryDetailsFacade : Controller
    {
        // GET: api/values
        public SalaryDetailsInterface __Salary;
        public SalaryDetailsFacade(SalaryDetailsInterface Salary)
        {
            __Salary = Salary;
        }


        // GET: api/values
        [Route("onloadgetdetails")]
        public SalaryDetailsDTO getinitialdata([FromBody]SalaryDetailsDTO dto)
        {
            return __Salary.getBasicData(dto);
        }
        [Route("Getdepartment")]
        public SalaryDetailsDTO Getdepartment([FromBody]SalaryDetailsDTO dto)
        {
            return __Salary.Getdepartment(dto);
        }
        [Route("get_designation")]
        public SalaryDetailsDTO get_designation([FromBody]SalaryDetailsDTO dto)
        {
            return __Salary.get_designation(dto);
        }
        [Route("get_department")]
        public SalaryDetailsDTO get_department([FromBody]SalaryDetailsDTO dto)
        {
            return __Salary.get_department(dto);
        }
        [Route("get_employee")]
        public SalaryDetailsDTO get_employee([FromBody]SalaryDetailsDTO dto)
        {
            return __Salary.get_employee(dto);
        }
        ////GetEmployeeDetailsByLeaveYearAndMonth

        [Route("GetEmployeeDetailsByLeaveYearAndMonth")]
        public SalaryDetailsDTO GetEmployeeDetailsByLeaveYearAndMonth([FromBody]SalaryDetailsDTO dto)
        {
            return __Salary.GetEmployeeDetailsByLeaveYearAndMonth(dto);
        }

        [HttpPost]
        [Route("GenerateEmployeeSalarySlip")]
        public Task<SalaryDetailsDTO> GenerateEmployeeSalarySlip([FromBody]SalaryDetailsDTO dto)
        {
            return __Salary.GenerateEmployeeSalarySlip(dto);
        }

       
    }
}
