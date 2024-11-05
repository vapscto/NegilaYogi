using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeInstallmentDetailsFacade : Controller
    {
        public FeeInstallmentDetailsInterface _feeinstdetail;
       

        public FeeInstallmentDetailsFacade(FeeInstallmentDetailsInterface feeinstdetail)
        {
            _feeinstdetail = feeinstdetail;
        }

        // GET: api/values

        [Route("getdetails")]
        public FeeInstallmentsDetailsDTO getdetails([FromBody]FeeInstallmentsDetailsDTO data)
        {
            return _feeinstdetail.getdetails(data);
        }

        
      
    }
}
