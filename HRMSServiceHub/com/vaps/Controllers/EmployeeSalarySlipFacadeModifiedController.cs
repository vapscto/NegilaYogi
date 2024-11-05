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
    public class EmployeeSalarySlipFacadeModifiedController : Controller
    {
        // GET: api/values
        public EmployeeSalarySlipInterfaceModified _ads;
        //public EmployeeSalarySlipGenerationInterface _ads;

        public EmployeeSalarySlipFacadeModifiedController(EmployeeSalarySlipInterfaceModified adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_Employee_SalaryModifiedDTO getinitialdata([FromBody]HR_Employee_SalaryModifiedDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        ////GetEmployeeDetailsByLeaveYearAndMonth

        [Route("GetEmployeeDetailsByLeaveYearAndMonth")]
        public HR_Employee_SalaryModifiedDTO GetEmployeeDetailsByLeaveYearAndMonth([FromBody]HR_Employee_SalaryModifiedDTO dto)
        {
            if (dto.MonthList.Length != 0)
            {
                return _ads.GetEmployeeDetailsByLeaveYearAndMultiMonth(dto);
            }
            else
            {
                return _ads.GetEmployeeDetailsByLeaveYearAndMonth(dto);
            }
        }

        [Route("GenerateEmployeeSalarySlip")]
        public Task<HR_Employee_SalaryModifiedDTO> getEmployeedetailsBySelection([FromBody]HR_Employee_SalaryModifiedDTO dto)
        {
            if (dto.MonthList.Length != 0)
            {
                return _ads.GenerateEmployeeSalarySlipMultiMonth(dto);
            }
            else
            {
                return _ads.GenerateEmployeeSalarySlip(dto);
            }
        }

        [Route("SendEmailSMS")]
        public async Task<HR_Employee_SalaryModifiedDTO> SendEmailSMS([FromBody]HR_Employee_SalaryModifiedDTO dto)
        {
            return await _ads.SendEmailSMS(dto);
        }

        [Route("get_depts")]
        public HR_Employee_SalaryModifiedDTO get_depts([FromBody]HR_Employee_SalaryModifiedDTO dto)
        {
            return _ads.get_depts(dto);
        }

        //[Route("get_Months")]
        //public HR_Employee_SalaryModifiedDTO get_Months([FromBody]HR_Employee_SalaryModifiedDTO dto)
        //{
        //    return _ads.get_Months(dto);
        //}

        [Route("get_desig")]
        public HR_Employee_SalaryModifiedDTO get_desig([FromBody]HR_Employee_SalaryModifiedDTO dto)
        {
            return _ads.get_desig(dto);
        }


    }
}
