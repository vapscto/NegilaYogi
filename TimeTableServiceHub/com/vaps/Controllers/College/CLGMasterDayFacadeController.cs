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
    public class CLGMasterDayFacadeController : Controller
    {
        public CLGMasterDayInterface _acd;
        public CLGMasterDayFacadeController(CLGMasterDayInterface acdm)
        {
            _acd = acdm;
        }
        [HttpPost]
        [Route("getalldetails")]
        public CLGMasterDayDTO getalldetails([FromBody]CLGMasterDayDTO data)
        {
            return _acd.getalldetails(data);
        }
        [HttpPost]
        [Route("editDay")]
        public CLGMasterDayDTO editDay([FromBody]CLGMasterDayDTO data)
        {
            return _acd.editDay(data);
        }

       
        [HttpPost]
        [Route("getBranch")]
        public CLGMasterDayDTO getBranch([FromBody]CLGMasterDayDTO data)
        {
          
            return _acd.getBranch(data);
          
        } 

        [HttpPost]
        [Route("saveday")]
        public CLGMasterDayDTO saveday([FromBody]CLGMasterDayDTO data)
        {
          
            return _acd.saveday(data);
          
        }

        [HttpPost]
        [Route("savesemday")]
        public CLGMasterDayDTO savedetails([FromBody]CLGMasterDayDTO data)
        {
          
            return _acd.savesemday(data);
          
        }
        [HttpPost]
        [Route("daydeactive")]
        public CLGMasterDayDTO daydeactive([FromBody]CLGMasterDayDTO data)
        {
            return _acd.daydeactive(data);
        }
        [HttpPost]
        [Route("deactivecrsday")]
        public CLGMasterDayDTO deactivecrsday([FromBody]CLGMasterDayDTO data)
        {
            return _acd.deactivecrsday(data);
        }
        [HttpPost]
        [Route("getorder")]
        public CLGMasterDayDTO getorder([FromBody]CLGMasterDayDTO data)
        {
            return _acd.getorder(data);
        }

        [HttpPost]
        [Route("saveorder")]
        public CLGMasterDayDTO saveorder([FromBody]CLGMasterDayDTO data)
        {
            return _acd.saveorder(data);
        }
     


      
    }
}
