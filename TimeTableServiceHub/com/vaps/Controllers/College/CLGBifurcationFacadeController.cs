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
    public class CLGBifurcationFacadeController : Controller
    {
        public CLGBifurcationInterface _acd;
        public CLGBifurcationFacadeController(CLGBifurcationInterface acdm)
        {
            _acd = acdm;
        }
        [HttpPost]
        [Route("getalldetails")]
        public CLGBifurcationDTO getalldetails([FromBody]CLGBifurcationDTO data)
        {
            return _acd.getalldetails(data);
        }
        [HttpPost]
        [Route("editDay")]
        public CLGBifurcationDTO editDay([FromBody]CLGBifurcationDTO data)
        {
            return _acd.editDay(data);
        }

       
        [HttpPost]
        [Route("getBranch")]
        public CLGBifurcationDTO getBranch([FromBody]CLGBifurcationDTO data)
        {
          
            return _acd.getBranch(data);
          
        } 

        [HttpPost]
        [Route("savedetailBiff")]
        public CLGBifurcationDTO savedetailBiff([FromBody]CLGBifurcationDTO data)
        {
          
            return _acd.savedetailBiff(data);
          
        }

        [HttpPost]
        [Route("editbiff")]
        public CLGBifurcationDTO editbiff([FromBody]CLGBifurcationDTO data)
        {
          
            return _acd.editbiff(data);
          
        }
        [HttpPost]
        [Route("deactivatebiff")]
        public CLGBifurcationDTO deactivatebiff([FromBody]CLGBifurcationDTO data)
        {
            return _acd.deactivatebiff(data);
        }
        [HttpPost]
        [Route("viewrecordspopup")]
        public CLGBifurcationDTO viewrecordspopup([FromBody]CLGBifurcationDTO data)
        {
            return _acd.viewrecordspopup(data);
        }
     


      
    }
}
