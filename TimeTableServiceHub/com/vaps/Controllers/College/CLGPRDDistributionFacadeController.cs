using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using TimeTableServiceHub.Interfaces;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTableServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class CLGPRDDistributionFacadeController : Controller
    {
        public CLGPRDDistributionInterface _acd;
        public CLGPRDDistributionFacadeController(CLGPRDDistributionInterface acdm)
        {
            _acd = acdm;
        }
        [HttpPost]
        [Route("getalldetails")]
        public CLGPRDDistributionDTO getalldetails([FromBody]CLGPRDDistributionDTO data)
        {
            return _acd.getalldetails(data);
        }
        [HttpPost]
        [Route("viewperiods")]
        public CLGPRDDistributionDTO viewperiods([FromBody]CLGPRDDistributionDTO data)
        {
            return _acd.viewperiods(data);
        }

       
        [HttpPost]
        [Route("getBranch")]
        public CLGPRDDistributionDTO getBranch([FromBody]CLGPRDDistributionDTO data)
        {
          
            return _acd.getBranch(data);
          
        } 

        [HttpPost]
        [Route("savedetail")]
        public CLGPRDDistributionDTO savedetail([FromBody]CLGPRDDistributionDTO data)
        {
          
            return _acd.savedetail(data);
          
        }

        [HttpPost]
        [Route("editprddestr")]
        public CLGPRDDistributionDTO editprddestr([FromBody]CLGPRDDistributionDTO data)
        {
          
            return _acd.editprddestr(data);
          
        }
        [HttpPost]
        [Route("deactivate")]
        public CLGPRDDistributionDTO deactivate([FromBody]CLGPRDDistributionDTO data)
        {
            return _acd.deactivate(data);
        }
        [HttpPost]
        [Route("deactivecrsday")]
        public CLGPRDDistributionDTO deactivecrsday([FromBody]CLGPRDDistributionDTO data)
        {
            return _acd.deactivecrsday(data);
        }
     


      
    }
}
