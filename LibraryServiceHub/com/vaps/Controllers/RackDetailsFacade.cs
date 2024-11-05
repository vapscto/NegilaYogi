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
    public class RackDetailsFacade : Controller
    {
        public RackDetailsInterface _objInter;
        public RackDetailsFacade (RackDetailsInterface para)
        {
            _objInter = para;
        }

        [Route("getdetails/{id:int}")]
        public RackDetailsDTO getdetails(int id)
        {
            return _objInter.getdetails(id);
        }
        [Route("Savedata")]
        public RackDetailsDTO Savedata([FromBody] RackDetailsDTO data)
        {
            return _objInter.Savedata(data);
        }
        [Route("EditData")]
        public RackDetailsDTO EditData([FromBody] RackDetailsDTO data)
        {
            return _objInter.EditData(data);
        }
        [Route("deactiveY")]
        public RackDetailsDTO deactiveY([FromBody]RackDetailsDTO data)
        {
            return _objInter.deactiveY(data);
        }
    }
}
