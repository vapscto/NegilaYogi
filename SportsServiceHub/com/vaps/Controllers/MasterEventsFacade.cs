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
    public class MasterEventsFacade : Controller
    {
        MasterEventsInterface _interface;
        public MasterEventsFacade(MasterEventsInterface interfaces)
        {
            _interface = interfaces;
        }
        [Route("getDetails")]
        public MasterEventsDTO getDetails([FromBody]MasterEventsDTO data)
        {
            return _interface.getDetails(data);
        }
        [Route("save")]
        public MasterEventsDTO save([FromBody]MasterEventsDTO data)
        {
            return _interface.saveRecord(data);
        }
        [Route("EditDetails")]
        public MasterEventsDTO EditDetails([FromBody] MasterEventsDTO data)
        {
            return _interface.EditDetails(data);
        }
        [Route("deactivate")]
        public MasterEventsDTO deactivateSponser([FromBody]MasterEventsDTO data)
        {
            return _interface.deactivate(data);
        }
    }
}
