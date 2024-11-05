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
    public class PDADueListReportFacade : Controller
    {
        public PDADueListReportInterface _pdamasterhead;

        public PDADueListReportFacade(PDADueListReportInterface maspag)
        {
            _pdamasterhead = maspag;
        }

        [Route("getalldetails")]
        public PDATransactionDTO getalldetails([FromBody]PDATransactionDTO data)
        {
            return _pdamasterhead.getalldetails(data);
        }


        [Route("getsection")]
        public PDATransactionDTO getsection([FromBody]PDATransactionDTO data)
        {
            return _pdamasterhead.getsection(data);
        }

        [Route("getstudent")]
        public PDATransactionDTO getstudent([FromBody]PDATransactionDTO data)
        {
            return _pdamasterhead.getstudent(data);
        }

        [Route("radiobtndata")]
        public Task<PDATransactionDTO> radiobtndata([FromBody]PDATransactionDTO data)
        {
            return _pdamasterhead.radiobtndata(data);
        }
    }
}
