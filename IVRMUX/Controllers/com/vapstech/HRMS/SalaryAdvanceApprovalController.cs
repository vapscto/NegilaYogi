using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.HRMS;
using Microsoft.AspNetCore.Http;
namespace corewebapi18072016.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class SalaryAdvanceApprovalController : Controller
    {
        SalaryAdvanceApprovalDelegate hrdel = new SalaryAdvanceApprovalDelegate();

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public HR_Emp_SalaryAdvanceDTO getalldetails(HR_Emp_SalaryAdvanceDTO dto)
        {
            //  HR_Emp_Loan_ApprovalDTO dto = new HR_Emp_Loan_ApprovalDTO();
            // dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return hrdel.onloadgetdetails(dto);
        }
    }
}