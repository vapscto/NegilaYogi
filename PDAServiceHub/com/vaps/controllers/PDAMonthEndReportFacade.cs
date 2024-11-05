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
    public class PDAMonthEndReportFacade : Controller
    {
        public PDAMonthEndReportInterface _pdamasterhead;

        public PDAMonthEndReportFacade(PDAMonthEndReportInterface maspag)
        {
            _pdamasterhead = maspag;
        }


        [HttpPost]
        [Route("getalldetails123")]
        public PDATransactionDTO Getdet([FromBody] PDATransactionDTO data)
        {
            return _pdamasterhead.getdata123(data);
        }

        [Route("getreport")]
        public Task<PDATransactionDTO> getreport([FromBody] PDATransactionDTO data)
        {
            return _pdamasterhead.getreport(data);
        }
    }
}
