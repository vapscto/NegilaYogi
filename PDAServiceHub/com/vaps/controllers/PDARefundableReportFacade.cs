using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PDAServiceHub.com.vaps.interfaces;
using PreadmissionDTOs.com.vaps.PDA;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PDAServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class PDARefundableReportFacade : Controller
    {
        public PDARefundableReportInterface _pdamasterhead;

        public PDARefundableReportFacade(PDARefundableReportInterface maspag)
        {
            _pdamasterhead = maspag;
        }

        [Route("getalldetails")]
        public PDATransactionDTO getalldetails([FromBody]PDATransactionDTO data)
        {
            return _pdamasterhead.getalldetails(data);
        }

        [Route("radiobtndata")]
        public Task<PDATransactionDTO> radiobtndata([FromBody]PDATransactionDTO data)
        {
            return _pdamasterhead.radiobtndata(data);
        }
    }
}
