using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using WebApplication1.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class TransactionNumberingFacadeController : Controller
    {

        public TransactionNumberingInterface _enq;

        public TransactionNumberingFacadeController(TransactionNumberingInterface enqui)
        {
            _enq = enqui;
        }

        // POST api/values
        [HttpPost]
        public Master_NumberingDTO Post([FromBody] Master_NumberingDTO Trans)

        {
            return _enq.saveMaster_Numbering(Trans);
        }

        [Route("getdetails")]
        public Master_NumberingDTO getTransactiongdet([FromBody]MandatoryFieldsDTO id)
        {
            return _enq.getdetails(id);
        }

        [Route("deleteRollnoconfig")]
        public Master_NumberingDTO deleteRollnoconfig([FromBody]Master_NumberingDTO id)
        {
            return _enq.deleteRollnoconfig(id);
        }

        

    }
}
