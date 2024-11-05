using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeSalaryCertificateFacadeController : Controller
    {
        public EmployeeSalaryCertificateInterface _ads;

        public EmployeeSalaryCertificateFacadeController(EmployeeSalaryCertificateInterface adstu)
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

        [Route("SendEmailSMS")]
        public async Task<HR_Employee_SalaryDTO> SendEmailSMS([FromBody]HR_Employee_SalaryDTO dto)
        {
            return await _ads.SendEmailSMS(dto);
        }


        [Route("GetEmployeeSalaryCertificate")]
        public HR_Employee_SalaryDTO GetEmployeeSalaryCertificate([FromBody]HR_Employee_SalaryDTO Dto)
        {
            return _ads.GetEmployeeSalaryCertificate(Dto);
        }
    }
}
