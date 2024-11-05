using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.PDA;
using PDAServiceHub.com.vaps.interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PDAServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class PDAMasterHeadFacade : Controller
    {
        public PDAMasterHeadInterface _pdamasterhead;

        public PDAMasterHeadFacade(PDAMasterHeadInterface maspag)
        {
            _pdamasterhead = maspag;
        }

        [Route("getdetails")]
        public PdaDTO getdetails([FromBody]PdaDTO data)
        {
            return _pdamasterhead.getdetails(data);
        }
        [Route("savedetails")]
        public PdaDTO savedetails([FromBody]PdaDTO data)
        {
            return _pdamasterhead.savedetails(data);
        }

        [Route("getpagedetails")]
        public PdaDTO getpagedetails([FromBody]PdaDTO data)
        {
            return _pdamasterhead.getpageedit(data);
        }

     
        [Route("deactivate")]
        public PdaDTO deactivate([FromBody]PdaDTO id)
        {
            return _pdamasterhead.deactivate(id);
        }

    }
}
