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
    public class EventVenueMappingController : Controller
    {
        EventVenueMappingDelegate delegat = new EventVenueMappingDelegate();

        [Route("getDetails/{id:int}")]
        public EventsMappingDTO getDetails(int id)
        {
            EventsMappingDTO dto = new EventsMappingDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delegat.getDetails(dto);
        }
        [Route("saveRecord")]
        public EventsMappingDTO saveRecord([FromBody]EventsMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.save(data);
        }
        [Route("EditDetails")]
        public EventsMappingDTO EditDetails([FromBody]EventsMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delegat.EditDetails(data);
        }
        [Route("deactivate")]
        public EventsMappingDTO deactivateSponser([FromBody] EventsMappingDTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegat.deactivate(d);
        }

        [Route("get_modeldata")]
        public EventsMappingDTO get_modeldata([FromBody] EventsMappingDTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //d.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delegat.get_modeldata(d);
        }

        [Route("Deactivesponsor")]
        public EventsMappingDTO Deactivesponsor([FromBody] EventsMappingDTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //d.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delegat.Deactivesponsor(d);
        }

    }
}
