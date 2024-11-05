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
    public class CLGBreakTimeSettingFacadeController : Controller
    {
        public CLGBreakTimeSettingInterface _acd;
        public CLGBreakTimeSettingFacadeController(CLGBreakTimeSettingInterface acdm)
        {
            _acd = acdm;
        }
        [HttpPost]
        [Route("getalldetails")]
        public CLGBreakTimeSettingDTO getalldetails([FromBody]CLGBreakTimeSettingDTO data)
        {
            return _acd.getalldetails(data);
        }
        [HttpPost]
        [Route("editDay")]
        public CLGBreakTimeSettingDTO editDay([FromBody]CLGBreakTimeSettingDTO data)
        {
            return _acd.editDay(data);
        }

       
        [HttpPost]
        [Route("getBranch")]
        public CLGBreakTimeSettingDTO getBranch([FromBody]CLGBreakTimeSettingDTO data)
        {
          
            return _acd.getBranch(data);
          
        } 

        [HttpPost]
        [Route("savetimedetail")]
        public CLGBreakTimeSettingDTO savetimedetail([FromBody]CLGBreakTimeSettingDTO data)
        {
          
            return _acd.savetimedetail(data);
          
        }

        [HttpPost]
        [Route("getmaximumperiodscount")]
        public CLGBreakTimeSettingDTO getmaximumperiodscount([FromBody]CLGBreakTimeSettingDTO data)
        {
          
            return _acd.getmaximumperiodscount(data);
          
        }
        [HttpPost]
        [Route("deactivate")]
        public CLGBreakTimeSettingDTO deactivate([FromBody]CLGBreakTimeSettingDTO data)
        {
            return _acd.deactivate(data);
        }
        [HttpPost]
        [Route("geteditdetails")]
        public CLGBreakTimeSettingDTO geteditdetails([FromBody]CLGBreakTimeSettingDTO data)
        {
            return _acd.geteditdetails(data);
        }
     


      
    }
}
