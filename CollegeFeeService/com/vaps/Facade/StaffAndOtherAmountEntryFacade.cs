using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeFeeService.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class StaffAndOtherAmountEntryFacade : Controller
    {
        public StaffAndOtherAmountEntryInterface _org;

        public StaffAndOtherAmountEntryFacade(StaffAndOtherAmountEntryInterface orga)
        {
            _org = orga;
        }


        [Route("getalldetails")]
        public FeeAmountEntryDTO Getdet([FromBody] FeeAmountEntryDTO data)
        {
            return _org.getdata(data);
        }
        [Route("Editdetails")]
        public FeeAmountEntryDTO Getmasterdetails(FeeAmountEntryDTO data)
        {
            return _org.EditMasterscetionDetails(data);
        }

        [HttpPost]
        [Route("paymentdetails")]
        public FeeAmountEntryDTO paymentdetailsfn([FromBody] FeeAmountEntryDTO id)
        {
            return _org.paymentdetailsfnc(id);
        }

        [Route("getgroupmappedheads")]
        public FeeAmountEntryDTO getgroupheaddetails([FromBody] FeeAmountEntryDTO pgmodu)
        {
            return _org.getgroupheaddetails(pgmodu);
        }


        public FeeAmountEntryDTO savedata([FromBody] FeeAmountEntryDTO pgmodu)
        {
            return _org.savedetails(pgmodu);
        }

        [HttpPost("{id}")]
        public FeeAmountEntryDTO Put(int id, [FromBody]FeeAmountEntryDTO value)
        {
            return _org.getsearchdata(id, value);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }


        [Route("deletemodpages")]
        public FeeAmountEntryDTO Delete([FromBody] FeeAmountEntryDTO data)
        {
            return _org.deleterec(data);
        }

        [Route("selectacademicyear")]
        public FeeAmountEntryDTO selectacade([FromBody] FeeAmountEntryDTO data)
        {
            return _org.selectacade(data);
        }
        [Route("getalldetailsOnselectiontype")]
        public FeeAmountEntryDTO getalldetailsOnselectiontype([FromBody] FeeAmountEntryDTO data)
        {
            return _org.getalldetailsOnselectiontype(data);
        }
    }
}
