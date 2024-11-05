

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Fees;

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeeStatusReportController: Controller
    {
        FeeStatusReportDelegate FSRD = new FeeStatusReportDelegate();
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeTransactionPaymentDTO Get(int id)
        {
            FeeTransactionPaymentDTO dto = new FeeTransactionPaymentDTO();
            dto.MI_ID= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FSRD.radiobtndata1(dto);
        }


        [HttpPost]
        [Route("get_fee_and_stu")]
        public FeeTransactionPaymentDTO get_fee_and_stu([FromBody]FeeTransactionPaymentDTO value)
        {
            value.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FSRD.get_fee_and_stu(value);
        }

        [HttpPost]
        [Route("radiobtndata")]
        public FeeTransactionPaymentDTO radiobtndata([FromBody]FeeTransactionPaymentDTO value)
        {
            value.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FSRD.radiobtndata(value);
        }
        [HttpPost]
        [Route("getfeehead_statement_report")]
        public FeeTransactionPaymentDTO getfeehead_statement_report([FromBody]FeeTransactionPaymentDTO value)
        {
            value.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FSRD.getfeehead_statement_report(value);
        }
        [HttpPost]
        [Route("getsection")]
        public FeeTransactionPaymentDTO getsection([FromBody]FeeTransactionPaymentDTO value)
        {
            value.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FSRD.getsection(value);
        }

        [Route("GetSettlementReport")]
        public FeeTransactionPaymentDTO GetSettlementReport([FromBody]FeeTransactionPaymentDTO value)
        {
            value.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FSRD.GetSettlementReport(value);
        }
    }
}
