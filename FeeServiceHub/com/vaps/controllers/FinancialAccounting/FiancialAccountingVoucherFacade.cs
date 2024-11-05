using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeeServiceHub.com.vaps.interfaces.FinancialAccounting;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees.FinancialAccounting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers.FinancialAccounting
{
    [Route("api/[controller]")]
    public class FiancialAccountingVoucherFacade : Controller
    {
        public FiancialAccountingVoucherInterface _org;

        public FiancialAccountingVoucherFacade(FiancialAccountingVoucherInterface orga)
        {
            _org = orga;
        }


        [Route("getalldetails")]
        public FiancialAccountingVoucherDTO getalldetails([FromBody] FiancialAccountingVoucherDTO data)
        {
            return _org.getdata(data);
        }

        [HttpPost]
        [Route("edit")]
        public FiancialAccountingVoucherDTO paymentdetailsfn([FromBody] FiancialAccountingVoucherDTO id)
        {
            return _org.edit(id);
        }
        [Route("savedata")]
        public FiancialAccountingVoucherDTO savedata([FromBody] FiancialAccountingVoucherDTO pgmodu)
        {
            return _org.savedetails(pgmodu);
        }
        //savedatatwo
        [Route("savedatatwo")]
        public FiancialAccountingVoucherDTO savedatatwo([FromBody] FiancialAccountingVoucherDTO pgmodu)
        {
            return _org.savedatatwo(pgmodu);
        }

        [Route("deletemodpages")]
        public FiancialAccountingVoucherDTO Delete([FromBody] FiancialAccountingVoucherDTO data)
        {
            return _org.deleterec(data);
        }

    }
}
