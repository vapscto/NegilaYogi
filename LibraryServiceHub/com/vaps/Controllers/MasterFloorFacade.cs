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
    public class MasterFloorFacade : Controller
    {
        public MasterFloorInterface _LibInter;
        public MasterFloorFacade(MasterFloorInterface para)
        {
            _LibInter = para;
        }
        [Route("Savedata")]
        public MasterFloorDTO Savedata([FromBody]MasterFloorDTO data)
        {
            return _LibInter.Savedata(data);
        }
        [Route("getdetails/{id:int}")]
        public MasterFloorDTO getdetails(int id)
        {
            return _LibInter.getdetails(id);
        }
        [Route("deactiveY")]
        public MasterFloorDTO deactiveY([FromBody]MasterFloorDTO data)
        {
            return _LibInter.deactiveY(data);
        }
    }
}
