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
    public class FAMasterGroupFacade : Controller
    {
        // public FeeAmountEntryInterfaces _org;
        public FAMasterGroupInterface _org;

        public FAMasterGroupFacade(FAMasterGroupInterface orga)
        {
            _org = orga;
        }

        
        [Route("getalldetails")]
        public FAMasterGroupDTO getalldetails([FromBody] FAMasterGroupDTO data)
        {
            return _org.getdata(data);
        }
       
        [HttpPost]
        [Route("edit")]
        public FAMasterGroupDTO paymentdetailsfn([FromBody] FAMasterGroupDTO id)
        {
            return _org.edit(id);
        }
        [Route("savedata")]
        public FAMasterGroupDTO savedata([FromBody] FAMasterGroupDTO pgmodu)
        {
            return _org.savedetails(pgmodu);
        }
        [Route("savedatatwo")]
        public FAMasterGroupDTO savedatatwo([FromBody] FAMasterGroupDTO pgmodu)
        {
            return _org.savedatatwo(pgmodu);
        }
        [Route("deletemodpages")]
        public FAMasterGroupDTO Delete([FromBody] FAMasterGroupDTO data)
        {
            return _org.deleterec(data);
        }

        }
}
