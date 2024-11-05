using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeSalaryDetailsFacadeController : Controller
    {
        public EmployeeSalaryDetailsInterface _ads;

        public EmployeeSalaryDetailsFacadeController(EmployeeSalaryDetailsInterface adstu)
            {
            _ads = adstu;
            }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_Employee_EarningsDeductionsDTO getinitialdata([FromBody]HR_Employee_EarningsDeductionsDTO dto)
            {
            return _ads.getBasicData(dto);
            }

        // POST api/values
        [HttpPost]
        public HR_Employee_EarningsDeductionsDTO Post([FromBody]HR_Employee_EarningsDeductionsDTO dto)
            {
            return _ads.SaveUpdate(dto);
            }

       
       

        //setemporder
        [Route("getEmployeeSalaryDetails")]
        public HR_Employee_EarningsDeductionsDTO getEmployeeSalaryDetails([FromBody]HR_Employee_EarningsDeductionsDTO dto)
            {
            return _ads.getEmployeeSalaryDetails(dto);
            }


        //setemporder
        [Route("getEmployeeSalaryDetailsByHead")]
        public HR_Employee_EarningsDeductionsDTO getEmployeeSalaryDetailsByHead([FromBody]HR_Employee_EarningsDeductionsDTO dto)
            {
            return _ads.getEmployeeSalaryDetailsByHead(dto);
            }

        [Route("GetEmployeeDetailsBySelected")]
        public HR_Employee_EarningsDeductionsDTO GetEmployeeDetailsBySelected([FromBody]HR_Employee_EarningsDeductionsDTO dto)
            {
            return _ads.GetEmployeeDetailsBySelected(dto);
            }

        [Route("get_depts")]
        public HR_Employee_EarningsDeductionsDTO get_depts([FromBody]HR_Employee_EarningsDeductionsDTO dto)
        {
            return _ads.get_depts(dto);
        }
        [Route("get_desig")]
        public HR_Employee_EarningsDeductionsDTO get_desig([FromBody]HR_Employee_EarningsDeductionsDTO dto)
        {
            return _ads.get_desig(dto);
        }

    }
}
