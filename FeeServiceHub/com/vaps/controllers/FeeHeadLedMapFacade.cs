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
    public class FeeHeadLedMapFacade : Controller
    {
        public FeeHeadLedMapInter _feegroupHeadpage;
        public FeeHeadLedMapFacade(FeeHeadLedMapInter maspag)
        {
            _feegroupHeadpage = maspag;
        }

        [Route("loaddata")]
        public HeadLedgerCodeMapDTO deactivate([FromBody] HeadLedgerCodeMapDTO data)
        {
            return _feegroupHeadpage.loaddata(data);
        }

        [Route("getheaddetails")]
        public HeadLedgerCodeMapDTO getheaddetails([FromBody] HeadLedgerCodeMapDTO data)
        {
            return _feegroupHeadpage.getheaddetails(data);
        }

        [Route("getgroupdetails")]
        public HeadLedgerCodeMapDTO getgroupdetails([FromBody] HeadLedgerCodeMapDTO data)
        {
            return _feegroupHeadpage.getgroupdetails(data);
        }

        [Route("savedata")]
        public HeadLedgerCodeMapDTO savedata([FromBody] HeadLedgerCodeMapDTO data)
        {
            return _feegroupHeadpage.savedata(data);
        }

        [Route("deletedata")]
        public HeadLedgerCodeMapDTO deletedata([FromBody] HeadLedgerCodeMapDTO data)
        {
            return _feegroupHeadpage.deletedata(data);
        }

    }
}
