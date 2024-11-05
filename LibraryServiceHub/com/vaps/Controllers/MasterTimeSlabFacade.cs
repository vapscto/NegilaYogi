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
    public class MasterTimeSlabFacade : Controller
    {
        public MasterTimeSlabInterface _objInter;
        public MasterTimeSlabFacade(MasterTimeSlabInterface para)
        {
            _objInter = para;
        }
        [Route("getdetails/{id:int}")]
        public MasterTimeSlabDTO getdetails(int id)
        {
            return _objInter.getdetails(id);
        }
        [Route("Savedata")]
        public MasterTimeSlabDTO Savedata([FromBody] MasterTimeSlabDTO data)
        {
            return _objInter.Savedata(data);
        }
        [Route("deactiveY")]
        public MasterTimeSlabDTO deactiveY([FromBody] MasterTimeSlabDTO data)
        {
            return _objInter.deactiveY(data);
        }
    }
}
