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
    public class LoannonDeductLetterFacadeController : Controller
    {


        // GET: api/values
        public LoannonDeductLetterInterface _ads;

        public LoannonDeductLetterFacadeController(LoannonDeductLetterInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("getalldetails")]
        public HR_Emp_Loan_TransactionDTO getalldetails([FromBody]HR_Emp_Loan_TransactionDTO dto)
        {
            return _ads.getBasicData(dto);
        }


        [HttpPost]
        public HR_Emp_Loan_TransactionDTO Post([FromBody]HR_Emp_Loan_TransactionDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }
        [Route("get_loans")]
        public HR_Emp_Loan_TransactionDTO get_loans([FromBody]HR_Emp_Loan_TransactionDTO dto)
        {
            return _ads.get_loans(dto);
        }
    }
}
