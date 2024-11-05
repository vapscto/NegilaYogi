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
    public class LoanApprovalFacadeController : Controller
    {
       // public LoanApprovalInterface _ads;

        public LoanApprovalInterface _ads;

        public LoanApprovalFacadeController(LoanApprovalInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("getalldetails")]
        public HR_Emp_Loan_ApprovalDTO getalldetails([FromBody]HR_Emp_Loan_ApprovalDTO dto)
        {
            return _ads.getBasicData(dto);
        }
    }
}
