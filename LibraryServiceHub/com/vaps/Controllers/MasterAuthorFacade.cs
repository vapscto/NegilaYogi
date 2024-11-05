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
    public class MasterAuthorFacade : Controller
    {
        public MasterAuthorInterface _objInter;
        public MasterAuthorFacade(MasterAuthorInterface data)
        {
            _objInter = data;
        }

        [Route("Savedata")]
        public LIB_Master_Author_DTO Savedata([FromBody] LIB_Master_Author_DTO data)
        {
            return _objInter.Savedata(data);
        }
        [Route("getdetails")]
        public LIB_Master_Author_DTO getdetails([FromBody]LIB_Master_Author_DTO id)
        {
            return _objInter.getdetails(id);
        }

        [Route("deactiveY")]
        public LIB_Master_Author_DTO deactiveY([FromBody] LIB_Master_Author_DTO data)
        {
            return _objInter.deactiveY(data);
        }
    }
}
