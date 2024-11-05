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
    public class CLGRestrictionFacadeController : Controller
    {
        public CLGRestrictionInterface _acd;
        public CLGRestrictionFacadeController(CLGRestrictionInterface acdm)
        {
            _acd = acdm;
        }

        //TAB1 START FIXING DAY PERIOD
        [HttpPost]
        [Route("getalldetails")]
        public CLGRestrictionDTO getalldetails([FromBody]CLGRestrictionDTO data)
        {
            return _acd.getalldetails(data);
        }
        [HttpPost]
        [Route("edittab1")]
        public CLGRestrictionDTO edittab1([FromBody]CLGRestrictionDTO data)
        {
            return _acd.edittab1(data);
        }


        [HttpPost]
        [Route("savetab1")]
        public CLGRestrictionDTO savetab1([FromBody]CLGRestrictionDTO data)
        {
          
            return _acd.savetab1(data);
          
        }
       
        [HttpPost]
        [Route("deactivatetab1")]
        public CLGRestrictionDTO deactivatetab1([FromBody]CLGRestrictionDTO data)
        {
            return _acd.deactivatetab1(data);
        }
        //TAB1 END FIXING DAY PERIOD

        //TAB2 START FIXING DAY STAFF

        [HttpPost]
        [Route("savetab2")]
        public CLGRestrictionDTO savetab2([FromBody]CLGRestrictionDTO data)
        {
            return _acd.savetab2(data);
        }
        [HttpPost]
        [Route("viewtab2grid")]
        public CLGRestrictionDTO viewtab2grid([FromBody]CLGRestrictionDTO data)
        {
            return _acd.viewtab2grid(data);
        }
        [HttpPost]
        [Route("gettab2editdata")]
        public CLGRestrictionDTO gettab2editdata([FromBody]CLGRestrictionDTO data)
        {
            return _acd.gettab2editdata(data);
        }

        [HttpPost]
        [Route("deactivatetab2")]
        public CLGRestrictionDTO deactivatetab2([FromBody]CLGRestrictionDTO data)
        {
            return _acd.deactivatetab2(data);
        }
        //TAB2 END FIXING DAY STAFF


        //TAB3 END FIXING DAY SUBJECT
        
               [HttpPost]
        [Route("savetab3")]
        public CLGRestrictionDTO savetab3([FromBody]CLGRestrictionDTO data)
        {
            return _acd.savetab3(data);
        }


        [HttpPost]
        [Route("viewtab3grid")]
        public CLGRestrictionDTO viewtab3grid([FromBody]CLGRestrictionDTO data)
        {
            return _acd.viewtab3grid(data);
        }
        [HttpPost]
        [Route("edittab3")]
        public CLGRestrictionDTO edittab3([FromBody]CLGRestrictionDTO data)
        {
            return _acd.edittab3(data);
        }
        [HttpPost]
        [Route("deactivatetab3")]
        public CLGRestrictionDTO deactivatetab3([FromBody]CLGRestrictionDTO data)
        {
            return _acd.deactivatetab3(data);
        }

        //TAB3 END FIXING DAY SUBJECT

        //TAB4 START FIXING PERIOD STAFF

        [HttpPost]
        [Route("savetab4")]
        public CLGRestrictionDTO savetab4([FromBody]CLGRestrictionDTO data)
        {
            return _acd.savetab4(data);
        }
        [HttpPost]
        [Route("viewtab4")]
        public CLGRestrictionDTO viewtab4([FromBody]CLGRestrictionDTO data)
        {
            return _acd.viewtab4(data);
        }
        [HttpPost]
        [Route("edittab4")]
        public CLGRestrictionDTO edittab4([FromBody]CLGRestrictionDTO data)
        {
            return _acd.edittab4(data);
        }
        [HttpPost]
        [Route("deactivatetab4")]
        public CLGRestrictionDTO deactivatetab4([FromBody]CLGRestrictionDTO data)
        {
            return _acd.deactivatetab4(data);
        }

        //TAB4 END FIXING PERIOD STAFF
        //TAB5 START FIXING PERIOD SUBJECT
        [HttpPost]
        [Route("savetab5")]
        public CLGRestrictionDTO savetab5([FromBody]CLGRestrictionDTO data)
        {
            return _acd.savetab5(data);
        }
        [HttpPost]
        [Route("viewtab5")]
        public CLGRestrictionDTO viewtab5([FromBody]CLGRestrictionDTO data)
        {
            return _acd.viewtab5(data);
        }
        [HttpPost]
        [Route("edittab5")]
        public CLGRestrictionDTO edittab5([FromBody]CLGRestrictionDTO data)
        {
            return _acd.edittab5(data);
        }
        [HttpPost]
        [Route("deactivatetab5")]
        public CLGRestrictionDTO deactivatetab5([FromBody]CLGRestrictionDTO data)
        {
            return _acd.deactivatetab5(data);
        }

        //TAB5 END FIXING PERIOD STAFF

    }
}
