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
    public class MasterDonorFacade : Controller
    {

        public MasterDonorInterface _objInter;
        public MasterDonorFacade(MasterDonorInterface para)
        {
            _objInter = para;
        }

        [Route("Savedata")]
        public MasterDonorDTO Savedata([FromBody]MasterDonorDTO data)
        {
            return _objInter.Savedata(data);
        }

        [Route("getdetails/{id:int}")]
        public MasterDonorDTO getdetails(int id)
        {
            return _objInter.getdetails(id);
        }
        [Route("deactiveY")]
        public MasterDonorDTO deactiveY([FromBody]MasterDonorDTO data)
        {
            return _objInter.deactiveY(data);
        }
    }
}
