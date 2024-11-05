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
    public class EventsSponsorController : Controller
    {
        EventsSponsorDelegate delegat = new EventsSponsorDelegate();
        [Route("loadgrid/{id:int}")]
        public EventsSponsorDTO getDetails(int id)
        {
            EventsSponsorDTO dto = new EventsSponsorDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.getDetails(dto);
        }
        [Route("saveRecord")]
        public EventsSponsorDTO saveRecord([FromBody]EventsSponsorDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.save(data);
        }
        [Route("Edit/{id:int}")]
        public EventsSponsorDTO Edit(int id)
        {
            return delegat.EditDetails(id);
        }
        [Route("deactivate")]
        public EventsSponsorDTO deactivateSponser([FromBody] EventsSponsorDTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.deactivate(d);
        }
    }
}
