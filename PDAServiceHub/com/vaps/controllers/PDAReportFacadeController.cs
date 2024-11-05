using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.PDA;
using PDAServiceHub.com.vaps.interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PDAServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class PDAReportFacadeController : Controller
    {
        PDAReportInterface _pdamasterhead;
        public PDAReportFacadeController(PDAReportInterface _inter)
        {
            _pdamasterhead = _inter;
        }

        [HttpPost]
        [Route("getalldetails")]
        public PDATransactionDTO getalldetails([FromBody]PDATransactionDTO data)
        {
            return _pdamasterhead.getalldetails(data);
        }
    }
}
