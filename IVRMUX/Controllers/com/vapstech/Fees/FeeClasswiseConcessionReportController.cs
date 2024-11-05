using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Fees;
using corewebapi18072016.Delegates.com.vapstech.Fees;

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    [ValidateAntiForgeryToken]
    public class FeeClasswiseConcessionReportController
    {
        FeeClasswiseConcessionReportDelegate FCWR = new FeeClasswiseConcessionReportDelegate();
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeTransactionPaymentDTO Get(int id)
        {
            return FCWR.getdetails(id);
        }

        [HttpPost]
        [Route("radiobtndata")]
        public FeeTransactionPaymentDTO radiobtndata([FromBody]FeeTransactionPaymentDTO value)
        {
            return FCWR.radiobtndata(value);
        }

    }
}
