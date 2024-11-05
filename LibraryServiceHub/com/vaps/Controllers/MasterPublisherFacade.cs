using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MasterPublisherFacade : Controller
    {
        public MasterPublisherInterface _objInter;
        public MasterPublisherFacade(MasterPublisherInterface para)
        {
            _objInter = para;
        }

        [Route("Savedata")]
        public MasterPublisherDTO Savedata([FromBody]MasterPublisherDTO data)
        {
            return _objInter.Savedata(data);
        }

        [Route("getdetails/{id:int}")]
        public MasterPublisherDTO getdetails(int id)
        {
            return _objInter.getdetails(id);
        }
        [Route("deactiveY")]
        public MasterPublisherDTO deactiveY([FromBody]MasterPublisherDTO data)
        {
            return _objInter.deactiveY(data);
        }

    }
}
