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
    public class LoanLetterRequestController : Controller
    {
        LoanLetterRequestDelegate hrdel = new LoanLetterRequestDelegate();

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public HR_Emp_Loan_TransactionDTO getalldetails(HR_Emp_Loan_TransactionDTO dto)
        {
            HR_Emp_Loan_TransactionDTO dtos = new HR_Emp_Loan_TransactionDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return hrdel.onloadgetdetails(dto);
        }

        [HttpPost]
        public HR_Emp_Loan_TransactionDTO Post([FromBody] HR_Emp_Loan_TransactionDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          //  dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return hrdel.savedetails(dto);
        }
        [Route("get_loans")]
        public HR_Emp_Loan_TransactionDTO get_loans([FromBody] HR_Emp_Loan_TransactionDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //  dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return hrdel.get_loans(dto);
        }

    }
}