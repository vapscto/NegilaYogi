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
    public class AutoLedgerCreationFacade : Controller
    {
        public AutoLedgerCreationInterface _org;

        public AutoLedgerCreationFacade(AutoLedgerCreationInterface orga)
        {
            _org = orga;
        }


        [Route("getalldetails")]
        public AutoLedgerCreationDTO getalldetails([FromBody] AutoLedgerCreationDTO data)
        {
            return _org.getdata(data);
        }

        [HttpPost]
        [Route("edit")]
        public AutoLedgerCreationDTO paymentdetailsfn([FromBody] AutoLedgerCreationDTO id)
        {
            return _org.edit(id);
        }
        [Route("savedata")]
        public AutoLedgerCreationDTO savedata([FromBody] AutoLedgerCreationDTO pgmodu)
        {
            return _org.savedetails(pgmodu);
        }
        //savedatatwo
        [Route("savedatatwo")]
        public AutoLedgerCreationDTO savedatatwo([FromBody] AutoLedgerCreationDTO pgmodu)
        {
            return _org.savedatatwo(pgmodu);
        }

        [Route("sectionchange")]
        public AutoLedgerCreationDTO sectionchange([FromBody] AutoLedgerCreationDTO pgmodu)
        {
            return _org.sectionchange(pgmodu);
        }

        [Route("deletemodpages")]
        public AutoLedgerCreationDTO Delete([FromBody] AutoLedgerCreationDTO data)
        {
            return _org.deleterec(data);
        }

    }

}
