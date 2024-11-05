using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SalaryApprovalFacadeController : Controller
    {
        public SalaryApprovalInterface _ads;

        public SalaryApprovalFacadeController(SalaryApprovalInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("getalldetails")]
        public HR_Employee_SalaryDTO getalldetails([FromBody]HR_Employee_SalaryDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        //[Route("getEmployeedetailsBySelection")]
        //public salaryApprovalFlowDTO getEmployeedetailsBySelection([FromBody]salaryApprovalFlowDTO dto)
        //{
        //    return _ads.getEmployeedetailsBySelection(dto);
        //}
    }
}