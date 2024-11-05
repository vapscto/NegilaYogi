using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeAmountEntryStthomasFacade : Controller
    {
        public FeeAmountEntryStthomasInterface _org;

        public FeeAmountEntryStthomasFacade(FeeAmountEntryStthomasInterface orga)
        {
            _org = orga;
        }


        [Route("getalldetails")]
        public FeeAmountEntryStthomasDTO Getdet([FromBody] FeeAmountEntryStthomasDTO data)
        {
            return _org.getdata(data);
        }
        [Route("Editdetails")]
        public FeeAmountEntryStthomasDTO Getmasterdetails(FeeAmountEntryStthomasDTO data)
        {
            return _org.EditMasterscetionDetails(data);
        }

        [HttpPost]
        [Route("paymentdetails")]
        public FeeAmountEntryStthomasDTO paymentdetailsfn([FromBody] FeeAmountEntryStthomasDTO id)
        {
            return _org.paymentdetailsfnc(id);
        }

        [Route("getgroupmappedheads")]
        public FeeAmountEntryStthomasDTO getgroupheaddetails([FromBody] FeeAmountEntryStthomasDTO pgmodu)
        {
            return _org.getgroupheaddetails(pgmodu);
        }


        public FeeAmountEntryStthomasDTO savedata([FromBody] FeeAmountEntryStthomasDTO pgmodu)
        {
           
            return _org.savedetails(pgmodu);
        }

        [HttpPost("{id}")]
        public FeeAmountEntryStthomasDTO Put(int id, [FromBody]FeeAmountEntryStthomasDTO value)
        {
            return _org.getsearchdata(id, value);
        }

        

        [Route("deletemodpages")]
        public FeeAmountEntryStthomasDTO Delete([FromBody] FeeAmountEntryStthomasDTO data)
        {
            return _org.deleterec(data);
        }

        [Route("selectacademicyear")]
        public FeeAmountEntryStthomasDTO selectacade([FromBody] FeeAmountEntryStthomasDTO data)
        {
            return _org.selectacade(data);
        }
        [Route("getalldetailsOnselectiontype")]
        public FeeAmountEntryStthomasDTO getalldetailsOnselectiontype([FromBody] FeeAmountEntryStthomasDTO data)
        {
            return _org.getalldetailsOnselectiontype(data);
        }
    }
}
