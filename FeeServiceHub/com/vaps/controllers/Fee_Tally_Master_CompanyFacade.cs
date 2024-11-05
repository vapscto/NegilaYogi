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
    public class Fee_Tally_Master_CompanyFacade : Controller
    {
        public Fee_Tally_Master_CompanyInterface _feegroupHeadpage;
        public Fee_Tally_Master_CompanyFacade(Fee_Tally_Master_CompanyInterface maspag)
        {
            _feegroupHeadpage = maspag;
        }

        [Route("loaddata")]
        public Fee_Tally_Master_CompanyDTO loaddata([FromBody] Fee_Tally_Master_CompanyDTO data)
        {
            return _feegroupHeadpage.loaddata(data);
        }
        [Route("savedata")]
        public Fee_Tally_Master_CompanyDTO savedata([FromBody] Fee_Tally_Master_CompanyDTO data)
        {
            return _feegroupHeadpage.savedata(data);
        }

        [Route("deletedata")]
        public Fee_Tally_Master_CompanyDTO deletedata([FromBody] Fee_Tally_Master_CompanyDTO data)
        {
            return _feegroupHeadpage.deletedata(data);
        }

    }
}
