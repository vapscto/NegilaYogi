using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using PortalHub.com.vaps.Employee.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Employee.Controllers
{
    [Route("api/[controller]")]
    public class SalarySlipFacade : Controller
    {
        // GET: api/values
        public SalarySlipInterface _work;
        public SalarySlipFacade(SalarySlipInterface work)
        {
            _work = work;
        }
        [Route("onloadgetdetails")]
        public HR_Employee_SalaryDTO getinitialdata([FromBody]HR_Employee_SalaryDTO dto)
        {
            return _work.getBasicData(dto);
        }

        ////GetEmployeeDetailsByLeaveYearAndMonth

        [Route("GetEmployeeDetailsByLeaveYearAndMonth")]
        public HR_Employee_SalaryDTO GetEmployeeDetailsByLeaveYearAndMonth([FromBody]HR_Employee_SalaryDTO dto)
        {
            return _work.GetEmployeeDetailsByLeaveYearAndMonth(dto);
        }


        [Route("GenerateEmployeeSalarySlip")]
        public Task<HR_Employee_SalaryDTO> getEmployeedetailsBySelection([FromBody]HR_Employee_SalaryDTO dto)
        {
            return _work.GenerateEmployeeSalarySlip(dto);
        }

          
    }
}
