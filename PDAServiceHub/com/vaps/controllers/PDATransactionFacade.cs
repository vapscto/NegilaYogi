using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.PDA;
using PDAServiceHub.com.vaps.interfaces;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PDAServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class PDATransactionFacade : Controller
    {
        public PDATransactionInterface _pdatransaction;

        public PDATransactionFacade(PDATransactionInterface maspag)
        {
            _pdatransaction = maspag;
        }


        [Route("getdetails")]
        public PDATransactionDTO getdetails([FromBody]PDATransactionDTO data)
        {
            return _pdatransaction.getdetails(data);
        }


        [Route("getsearchfilter")]
        public FeeStudentTransactionDTO getsearchfilter([FromBody]FeeStudentTransactionDTO data)
        {
            return _pdatransaction.getsearchfilter(data);
        }

        [Route("Savedata")]
        public PDATransactionDTO Savedata([FromBody]PDATransactionDTO data)
        {
            return _pdatransaction.Savedata(data);
        }

        [Route("searching")]
        public PDATransactionDTO searching([FromBody]PDATransactionDTO data)
        {
            return _pdatransaction.searching(data);
        }


        [Route("Deletedetails")]
        public PDATransactionDTO Deletedetails([FromBody]PDATransactionDTO data)
        {
            return _pdatransaction.Deletedetails(data);
        }
        [Route("getsection")]
        public PDATransactionDTO getsection([FromBody]PDATransactionDTO data)
        {
            return _pdatransaction.getsection(data);
        }
        [Route("getstudent")]
        public PDATransactionDTO getstudent([FromBody]PDATransactionDTO data)
        {
            return _pdatransaction.getstudent(data);
        }


    }


}
