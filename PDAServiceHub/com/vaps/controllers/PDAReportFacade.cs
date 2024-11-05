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
    public class PDAReportFacade : Controller
    {
        public PDAReportInterface _PdaheadContext;

        public PDAReportFacade(PDAReportInterface maspag)
        {
            _PdaheadContext = maspag;
        }

        [Route("getalldetails")]
        public PDATransactionDTO getalldetails([FromBody]PDATransactionDTO data)
        {
            return _PdaheadContext.getalldetails(data);
        }

        [Route("radiobtndata")]
        public Task<PDATransactionDTO> radiobtndata([FromBody]PDATransactionDTO data)
        {
            return _PdaheadContext.radiobtndata(data);
        }
       
        [Route("getsection")]
        public PDATransactionDTO getsection([FromBody]PDATransactionDTO data)
        {
            return _PdaheadContext.getsection(data);
        }

        [Route("getstudent")]
        public PDATransactionDTO getstudent([FromBody]PDATransactionDTO data)
        {
            return _PdaheadContext.getstudent(data);
        }


    }
}
