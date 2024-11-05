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
    public class CLGFixingFacadeController : Controller
    {
        public CLGFixingInterface _acd;
        public CLGFixingFacadeController(CLGFixingInterface acdm)
        {
            _acd = acdm;
        }

        //TAB1 START FIXING DAY PERIOD
        [HttpPost]
        [Route("getalldetails")]
        public CLGFixingDTO getalldetails([FromBody]CLGFixingDTO data)
        {
            return _acd.getalldetails(data);
        }
        [HttpPost]
        [Route("edittab1")]
        public CLGFixingDTO edittab1([FromBody]CLGFixingDTO data)
        {
            return _acd.edittab1(data);
        }


        [HttpPost]
        [Route("savetab1")]
        public CLGFixingDTO savetab1([FromBody]CLGFixingDTO data)
        {
          
            return _acd.savetab1(data);
          
        }
       
        [HttpPost]
        [Route("deactivatetab1")]
        public CLGFixingDTO deactivatetab1([FromBody]CLGFixingDTO data)
        {
            return _acd.deactivatetab1(data);
        }
        //TAB1 END FIXING DAY PERIOD

        //TAB2 START FIXING DAY STAFF

        [HttpPost]
        [Route("savetab2")]
        public CLGFixingDTO savetab2([FromBody]CLGFixingDTO data)
        {
            return _acd.savetab2(data);
        }
        [HttpPost]
        [Route("viewtab2grid")]
        public CLGFixingDTO viewtab2grid([FromBody]CLGFixingDTO data)
        {
            return _acd.viewtab2grid(data);
        }
        [HttpPost]
        [Route("gettab2editdata")]
        public CLGFixingDTO gettab2editdata([FromBody]CLGFixingDTO data)
        {
            return _acd.gettab2editdata(data);
        }

        [HttpPost]
        [Route("deactivatetab2")]
        public CLGFixingDTO deactivatetab2([FromBody]CLGFixingDTO data)
        {
            return _acd.deactivatetab2(data);
        }
        //TAB2 END FIXING DAY STAFF


        //TAB3 END FIXING DAY SUBJECT
        
               [HttpPost]
        [Route("savetab3")]
        public CLGFixingDTO savetab3([FromBody]CLGFixingDTO data)
        {
            return _acd.savetab3(data);
        }


        [HttpPost]
        [Route("viewtab3grid")]
        public CLGFixingDTO viewtab3grid([FromBody]CLGFixingDTO data)
        {
            return _acd.viewtab3grid(data);
        }
        [HttpPost]
        [Route("edittab3")]
        public CLGFixingDTO edittab3([FromBody]CLGFixingDTO data)
        {
            return _acd.edittab3(data);
        }
        [HttpPost]
        [Route("deactivatetab3")]
        public CLGFixingDTO deactivatetab3([FromBody]CLGFixingDTO data)
        {
            return _acd.deactivatetab3(data);
        }

        //TAB3 END FIXING DAY SUBJECT

        //TAB4 START FIXING PERIOD STAFF

        [HttpPost]
        [Route("savetab4")]
        public CLGFixingDTO savetab4([FromBody]CLGFixingDTO data)
        {
            return _acd.savetab4(data);
        }
        [HttpPost]
        [Route("viewtab4")]
        public CLGFixingDTO viewtab4([FromBody]CLGFixingDTO data)
        {
            return _acd.viewtab4(data);
        }
        [HttpPost]
        [Route("edittab4")]
        public CLGFixingDTO edittab4([FromBody]CLGFixingDTO data)
        {
            return _acd.edittab4(data);
        }
        [HttpPost]
        [Route("deactivatetab4")]
        public CLGFixingDTO deactivatetab4([FromBody]CLGFixingDTO data)
        {
            return _acd.deactivatetab4(data);
        }

        //TAB4 END FIXING PERIOD STAFF
        //TAB5 START FIXING PERIOD SUBJECT
        [HttpPost]
        [Route("savetab5")]
        public CLGFixingDTO savetab5([FromBody]CLGFixingDTO data)
        {
            return _acd.savetab5(data);
        }
        [HttpPost]
        [Route("viewtab5")]
        public CLGFixingDTO viewtab5([FromBody]CLGFixingDTO data)
        {
            return _acd.viewtab5(data);
        }
        [HttpPost]
        [Route("edittab5")]
        public CLGFixingDTO edittab5([FromBody]CLGFixingDTO data)
        {
            return _acd.edittab5(data);
        }
        [HttpPost]
        [Route("deactivatetab5")]
        public CLGFixingDTO deactivatetab5([FromBody]CLGFixingDTO data)
        {
            return _acd.deactivatetab5(data);
        }

        //TAB5 END FIXING PERIOD STAFF

    }
}
