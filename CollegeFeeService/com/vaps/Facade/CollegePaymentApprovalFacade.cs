using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeFeeService.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class CollegePaymentApprovalFacade : Controller
    {
        public CollegePaymentApprovalInterface _feedefaulters;

        public CollegePaymentApprovalFacade(CollegePaymentApprovalInterface maspag)
        {
            _feedefaulters = maspag;
        }




      //  [HttpGet]
        [Route("getdetails")]
        public CollegePaymentApprovalDTO getdetails([FromBody] CollegePaymentApprovalDTO data)
        {
            return _feedefaulters.getdetails(data);
        }
        [HttpPost]
        [Route("Getreportdetails")]
        public CollegePaymentApprovalDTO Getreportdetails([FromBody]CollegePaymentApprovalDTO temp)
        {
            return _feedefaulters.Getreportdetails(temp);
        }
        [HttpPost]
        [Route("savedetails")]
        public CollegePaymentApprovalDTO savedetails([FromBody]CollegePaymentApprovalDTO temp)
        {
            return _feedefaulters.savedetails(temp);
        }
    }
}
