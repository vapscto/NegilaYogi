using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;
using LibraryServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MasterPeriodicityFacade : Controller
    {

        MasterPeriodicityInterface _objinter;
        public MasterPeriodicityFacade(MasterPeriodicityInterface para)
        {
            _objinter = para;
        }

      [Route("Savedata")]
      public MasterPeriodicityDTO Savedata([FromBody]MasterPeriodicityDTO data)
        {
            return _objinter.Savedata(data);
        }
        [Route("getdetails/{id:int}")]
        public MasterPeriodicityDTO getdetails(int id)
        {
            return _objinter.getdetails(id);
        }
        [Route("deactiveY")]
        public MasterPeriodicityDTO deactiveY([FromBody]MasterPeriodicityDTO data)
        {
            return _objinter.deactiveY(data);
        }
    }
}
