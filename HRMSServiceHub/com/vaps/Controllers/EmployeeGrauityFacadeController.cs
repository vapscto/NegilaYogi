using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeGrauityFacadeController : Controller
    {
        // GET: api/values
        public EmployeeGrauityInterface _ads;

        public EmployeeGrauityFacadeController(EmployeeGrauityInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_Employee_SalaryDTO getinitialdata([FromBody]HR_Employee_SalaryDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        ////GetEmployeeDetailsByLeaveYearAndMonth

        [Route("GetEmployeeDetailsByLeaveYearAndMonth")]
        public HR_Employee_SalaryDTO GetEmployeeDetailsByLeaveYearAndMonth([FromBody]HR_Employee_SalaryDTO dto)
        {
            return _ads.GetEmployeeDetailsByLeaveYearAndMonth(dto);
        }


        [Route("GenerateEmployeeSalarySlip")]
        public Task<HR_Employee_SalaryDTO> getEmployeedetailsBySelection([FromBody]HR_Employee_SalaryDTO dto)
        {
            return _ads.GenerateEmployeeSalarySlip(dto);
        }

       


        }
}
