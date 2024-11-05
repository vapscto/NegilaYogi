using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees.Tally;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeTallyTransactionFacade : Controller
    {
        public FeeTallyTransactionInterface _feegroupHeadpage;
        public FeeTallyTransactionFacade(FeeTallyTransactionInterface maspag)
        {
            _feegroupHeadpage = maspag;
        }

        [Route("loaddata")]
        public TallyMTransactionDTO deactivate([FromBody] TallyMTransactionDTO data)
        {
            return _feegroupHeadpage.loaddata(data);
        }

        [Route("getstudentdetails")]
        public TallyMTransactionDTO gethegetstudentdetailsaddetails([FromBody] TallyMTransactionDTO data)
        {
            return _feegroupHeadpage.getstudentdetails(data);
        }

        [Route("savedata")]
        public TallyMTransactionDTO savedata([FromBody] TallyMTransactionDTO data)
        {
            return _feegroupHeadpage.savedata(data);
        }

        [Route("deletedata")]
        public TallyMTransactionDTO deletedata([FromBody] TallyMTransactionDTO data)
        {
            return _feegroupHeadpage.deletedata(data);
        }

        [Route("Paymentdata")]
        public TallyMTransactionDTO Paymentdata([FromBody] TallyMTransactionDTO data)
        {
            return _feegroupHeadpage.Paymentdata(data);
        }

        [Route("Concessiondata")]
        public TallyMTransactionDTO Concessiondata([FromBody] TallyMTransactionDTO data)
        {
            return _feegroupHeadpage.Concessiondata(data);
        }

        //[Route("getalldetails/{MI_Id:long}")]
        //public TallyMTransactionDTO getalldetails(long MI_Id)
        //{
        //    return _feegroupHeadpage.getalldetails(MI_Id);
        //}

        [Route("getalldetails")] 
        public TallyMTransactionDTO getalldetails([FromBody] TallyMTransactionDTO data)
        {
            return _feegroupHeadpage.getalldetails(data);
        }


        [Route("getvouchertypedetails")]
        public TallyMTransactionDTO getvouchertypedetails([FromBody] TallyMTransactionDTO data)
        {
            return _feegroupHeadpage.getvouchertypedetails(data);
        }


        //=============tally reports================== 
        [Route("get_tally_data")]
        public TallyMTransactionDTO get_tally_data([FromBody] TallyMTransactionDTO data)
        {
            return _feegroupHeadpage.get_tally_data(data);
        }

    }
}
