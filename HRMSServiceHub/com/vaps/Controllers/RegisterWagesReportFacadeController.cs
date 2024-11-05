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
    public class RegisterWagesReportFacadeController : Controller
    {
        public RegisterWagesInterface _ads;

        public RegisterWagesReportFacadeController(RegisterWagesInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_EmployeeRegisterDTO getinitialdata([FromBody]HR_EmployeeRegisterDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        [Route("getEmployeedetailsBySelection")]
        public Task<HR_EmployeeRegisterDTO> getEmployeedetailsBySelection([FromBody]HR_EmployeeRegisterDTO dto)
        {
            return _ads.getEmployeedetailsBySelection(dto);
        }
        [Route("get_depts")]
        public HR_EmployeeRegisterDTO get_depts([FromBody]HR_EmployeeRegisterDTO dto)
        {
            return _ads.get_depts(dto);
        }
        [Route("get_desig")]
        public HR_EmployeeRegisterDTO get_desig([FromBody]HR_EmployeeRegisterDTO dto)
        {
            return _ads.get_desig(dto);
        }
    }
}

