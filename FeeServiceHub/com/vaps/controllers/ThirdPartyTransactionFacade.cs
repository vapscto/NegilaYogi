using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fee;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class ThirdPartyTransactionFacade : Controller
    {

        public ThirdPartyTransactionInterface objInt;
        public ThirdPartyTransactionFacade(ThirdPartyTransactionInterface interf)
        {
            objInt = interf;
        }

        // GET: api/values
        [HttpPost]
        [Route("getdetails")]
        public ThirdPartyTransactionDTO getalldetails([FromBody] ThirdPartyTransactionDTO data)
        {
            return objInt.getdetails(data);
        }

        //  [HttpPost]  
        [Route("getgrpdetails")]
        public ThirdPartyTransactionDTO getgrpdetails([FromBody] ThirdPartyTransactionDTO data)
        {
            return objInt.getgrpdetails(data);
        }

        [Route("getStudtdetails")]
        public ThirdPartyTransactionDTO getStudtdetails([FromBody] ThirdPartyTransactionDTO data)
        {
            return objInt.getStudtdetails(data);
        }

        //SaveStudentgroupdata
        [Route("SaveStudentgroupdata")]
        public ThirdPartyTransactionDTO SaveStudentgroupdata([FromBody] ThirdPartyTransactionDTO data)
        {
            return objInt.SaveStudentgroupdata(data);
        }
        [Route("Ckeck_Receipt")]
        public ThirdPartyTransactionDTO Ckeck_Receipt([FromBody] ThirdPartyTransactionDTO data)
        {
            return objInt.Ckeck_Receipt(data);
        }

        //editOthtransaction
        [Route("editOthtransaction")]
        public ThirdPartyTransactionDTO editOthtransaction([FromBody] ThirdPartyTransactionDTO data)
        {
            return objInt.editOthtransaction(data);
        }

        [Route("DeletOthrRecord")]
        public ThirdPartyTransactionDTO DeletOthrRecord([FromBody] ThirdPartyTransactionDTO data)
        {
            return objInt.DeletOthrRecord(data);
        }

        [Route("printreceipt")]
        public ThirdPartyTransactionDTO printreceipt([FromBody] ThirdPartyTransactionDTO data)
        {
            return objInt.printreceipt(data);
        }

    }
}
