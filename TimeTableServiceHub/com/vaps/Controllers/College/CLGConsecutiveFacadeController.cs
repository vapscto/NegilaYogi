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
    public class CLGConsecutiveFacadeController : Controller
    {
        public CLGConsecutiveInterface _acd;
        public CLGConsecutiveFacadeController(CLGConsecutiveInterface acdm)
        {
            _acd = acdm;
        }
        [HttpPost]
        [Route("getalldetails")]
        public CLGConsecutiveDTO getalldetails([FromBody]CLGConsecutiveDTO data)
        {
            return _acd.getalldetails(data);
        }
        [HttpPost]
        [Route("editconv")]
        public CLGConsecutiveDTO editconv([FromBody]CLGConsecutiveDTO data)
        {
            return _acd.editconv(data);
        }


        [HttpPost]
        [Route("savedetail")]
        public CLGConsecutiveDTO savedetail([FromBody]CLGConsecutiveDTO data)
        {
          
            return _acd.savedetail(data);
          
        }

        
        [HttpPost]
        [Route("deactivate")]
        public CLGConsecutiveDTO deactivate([FromBody]CLGConsecutiveDTO data)
        {
            return _acd.deactivate(data);
        }
       
    }
}
