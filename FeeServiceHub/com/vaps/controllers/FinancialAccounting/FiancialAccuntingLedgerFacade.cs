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
    public class FiancialAccuntingLedgerFacade : Controller
    {
        public FiancialAccuntingLedgerInterface _org;

        public FiancialAccuntingLedgerFacade(FiancialAccuntingLedgerInterface orga)
        {
            _org = orga;
        }


        [Route("getalldetails")]
        public FiancialAccuntingLedgerDTO getalldetails([FromBody] FiancialAccuntingLedgerDTO data)
        {
            return _org.getdata(data);
        }

        [HttpPost]
        [Route("edit")]
        public FiancialAccuntingLedgerDTO paymentdetailsfn([FromBody] FiancialAccuntingLedgerDTO id)
        {
            return _org.edit(id);
        }
        [Route("savedata")]
        public FiancialAccuntingLedgerDTO savedata([FromBody] FiancialAccuntingLedgerDTO pgmodu)
        {
            return _org.savedetails(pgmodu);
        }
        //savedatatwo
        [Route("savedatatwo")]
        public FiancialAccuntingLedgerDTO savedatatwo([FromBody] FiancialAccuntingLedgerDTO pgmodu)
        {
            return _org.savedatatwo(pgmodu);
        }

        [Route("deletemodpages")]
        public FiancialAccuntingLedgerDTO Delete([FromBody] FiancialAccuntingLedgerDTO data)
        {
            return _org.deleterec(data);
        }

    }
}
