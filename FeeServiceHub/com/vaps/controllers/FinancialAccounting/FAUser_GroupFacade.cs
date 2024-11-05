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
    public class FAUser_GroupFacade : Controller
    {
        public FAUser_GroupInterface _org;

        public FAUser_GroupFacade(FAUser_GroupInterface orga)
        {
            _org = orga;
        }


        [Route("getalldetails")]
        public FAUser_GroupDTO getalldetails([FromBody] FAUser_GroupDTO data)
        {
            return _org.getdata(data);
        }

        [HttpPost]
        [Route("edit")]
        public FAUser_GroupDTO paymentdetailsfn([FromBody] FAUser_GroupDTO id)
        {
            return _org.edit(id);
        }
        [Route("savedata")]
        public FAUser_GroupDTO savedata([FromBody] FAUser_GroupDTO pgmodu)
        {
            return _org.savedetails(pgmodu);
        }
        //savedatatwo
        [Route("savedatatwo")]
        public FAUser_GroupDTO savedatatwo([FromBody] FAUser_GroupDTO pgmodu)
        {
            return _org.savedatatwo(pgmodu);
        }

        [Route("deletemodpages")]
        public FAUser_GroupDTO Delete([FromBody] FAUser_GroupDTO data)
        {
            return _org.deleterec(data);
        }
        //Userchange
        [Route("Userchange")]
        public FAUser_GroupDTO Userchange([FromBody] FAUser_GroupDTO data)
        {
            return _org.Userchange(data);
        }

    }
}
