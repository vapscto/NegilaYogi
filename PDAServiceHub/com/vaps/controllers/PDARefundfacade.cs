using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PDAServiceHub.com.vaps.interfaces;
using PreadmissionDTOs.com.vaps.PDA;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PDAServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class PDARefundfacade : Controller
    {
        public PDARefundInterface _pdamasterhead;

        public PDARefundfacade(PDARefundInterface maspag)
        {
            _pdamasterhead = maspag;
        }
        
        [Route("getdetails")]
        public PDATransactionDTO getdetails([FromBody]PDATransactionDTO data)
        {
            return _pdamasterhead.getdetails(data);
        }

        [Route("getsearchfilter")]
        public FeeStudentTransactionDTO getsearchfilter([FromBody]FeeStudentTransactionDTO data)
        {
            return _pdamasterhead.getsearchfilter(data);
        }

        [Route("getstuddetails")]
        public PDATransactionDTO getstuddetails([FromBody]PDATransactionDTO data)
        {
            return _pdamasterhead.getstuddetails(data);
        }


        [Route("Savedata")]
        public PDATransactionDTO Savedata([FromBody]PDATransactionDTO data)
        {
            return _pdamasterhead.Savedata(data);
        }

        [Route("searching")]
        public PDATransactionDTO searching([FromBody]PDATransactionDTO data)
        {
            return _pdamasterhead.searching(data);
        }

        [Route("Deletedetails")]
        public PDATransactionDTO Deletedetails([FromBody]PDATransactionDTO data)
        {
            return _pdamasterhead.Deletedetails(data);
        }


        [Route("getacademicyear")]
        public PDATransactionDTO Getstudacademic([FromBody] PDATransactionDTO data)
        {
            return _pdamasterhead.getdatastuacad(data);
        }



    }

}
