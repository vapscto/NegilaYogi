using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Sports;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class EventsSponsorFacade : Controller
    {
        EventsSponsorInterface _interface;
        public EventsSponsorFacade(EventsSponsorInterface interfaces)
        {
            _interface = interfaces;
        }
        [Route("getDetails")]
        public EventsSponsorDTO getDetails([FromBody]EventsSponsorDTO data)
        {
            return _interface.getDetails(data);
        }
        [Route("save")]
        public EventsSponsorDTO save([FromBody]EventsSponsorDTO data)
        {
            return _interface.saveRecord(data);
        }
        [Route("EditDetails/{id:int}")]
        public EventsSponsorDTO EditDetails(int id)
        {
            return _interface.EditDetails(id);
        }
        [Route("deactivate")]
        public EventsSponsorDTO deactivate([FromBody]EventsSponsorDTO data)
        {
            return _interface.deactivate(data);
        }
    }
}
