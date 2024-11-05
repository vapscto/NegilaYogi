using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Sports;
using PreadmissionDTOs.com.vaps.Sports;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class MasterEventController : Controller
    {
        MasterEventDelegate delegat = new MasterEventDelegate();


        [Route("getDetails/{id:int}")]
        public MasterEventsDTO getDetails(int id)
        {
            MasterEventsDTO data = new MasterEventsDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.getDetails(data);
        }
        [Route("saveRecord")]
        public MasterEventsDTO saveRecord([FromBody]MasterEventsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.save(data);
        }
        [Route("EditDetails")]
        public MasterEventsDTO EditDetails([FromBody] MasterEventsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delegat.EditDetails(data);
        }
        [Route("deactivate")]
        public MasterEventsDTO deactivateSponser([FromBody] MasterEventsDTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.deactivate(d);
        }
    }
}
