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
    public class Form16FacadeController : Controller
    {
        // GET: api/values
        public Form16Interface _ads;

        public Form16FacadeController(Form16Interface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public Form16DTO getinitialdata([FromBody]Form16DTO dto)
        {
            return _ads.getBasicData(dto);
        }

        ////GetEmployeeDetailsByLeaveYearAndMonth

        [Route("GetEmployeeDetailsByLeaveYearAndMonth")]
        public Form16DTO GetEmployeeDetailsByLeaveYearAndMonth([FromBody]Form16DTO dto)
        {
            return _ads.GetEmployeeDetailsByLeaveYearAndMonth(dto);
        }


        [Route("GenerateEmployeeSalarySlip")]
        public Task<Form16DTO> getEmployeedetailsBySelection([FromBody]Form16DTO dto)
        {
            return _ads.GenerateEmployeeSalarySlip(dto);
        }

       


        }
}
