using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class feeitreportFacade : Controller
    {
        public feeitreportInterface _feeit;
        public feeitreportFacade(feeitreportInterface maspag)
        {
            _feeit = maspag;
        }

        [Route("getdetails")]
        public FeeTransactionPaymentDTO getdetails([FromBody]FeeTransactionPaymentDTO dt)
        {
            return _feeit.getdetails(dt);
        }
        [Route("getsection")]
        public FeeTransactionPaymentDTO getsection([FromBody]FeeTransactionPaymentDTO data)
        {
            return _feeit.getsection(data);
        }
        [Route("getstudent")]
        public FeeTransactionPaymentDTO getstudent([FromBody]FeeTransactionPaymentDTO data)
        {
            return _feeit.getstudent(data);
        }
        [Route("getreceipt")]
        public FeeTransactionPaymentDTO getreceipt([FromBody]FeeTransactionPaymentDTO data)
        {
            return _feeit.getreceipt(data);
        }
        [Route("printreceipt")]
        public Task<FeeStudentTransactionDTO> printreceipt([FromBody] FeeStudentTransactionDTO data)
        {
            return _feeit.printreceipt(data);
        }

        [Route("getreceiptreport")]
        public FeeTransactionPaymentDTO getreceiptreport([FromBody]FeeTransactionPaymentDTO data)
        {
            return _feeit.getreceiptreport(data);
        }
    }
}
