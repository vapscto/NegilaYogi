using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeeServiceHub.com.vaps.interfaces;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class BankDetailsFacade : Controller
    {
        public BankDetailsInterface _org;

        public BankDetailsFacade(BankDetailsInterface orga)
        {
            _org = orga;
        }

        [HttpPost]
        [Route("getalldetails")]
        public BankDetailsDTO getalldetails([FromBody] BankDetailsDTO data)
        {
            return _org.getalldetails(data);
        }
        [Route("getdata")]
        public BankDetailsDTO getdata([FromBody] BankDetailsDTO data)
        {
            return _org.getdata(data);
        }
        [Route("edittab1")]
        public BankDetailsDTO edittab1([FromBody]  BankDetailsDTO data)
        {
            return _org.edittab1(data);
        }
        [Route("deactive")]
        public BankDetailsDTO deactive([FromBody] BankDetailsDTO data)
        {
            return _org.deactive(data);
        }

    }
        
    
}
