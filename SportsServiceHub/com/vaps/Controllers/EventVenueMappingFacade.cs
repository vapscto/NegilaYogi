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
    public class EventVenueMappingFacade : Controller
    {
        EventVenueMappingInterface _interface;
        public EventVenueMappingFacade(EventVenueMappingInterface interfaces)
        {
            _interface = interfaces;
        }

        [Route("getDetails")]
        public EventsMappingDTO getDetails([FromBody]EventsMappingDTO data)
        {
            return _interface.getDetails(data);
        }

        [Route("save")]
        public EventsMappingDTO save([FromBody]EventsMappingDTO data)
        {
            return _interface.saveRecord(data);
        }

        [Route("EditDetails")]
        public Task<EventsMappingDTO> EditDetails([FromBody]EventsMappingDTO id)
        {
            return _interface.EditDetails(id);
        }

        [Route("deactivate")]
        public EventsMappingDTO deactivateSponser([FromBody]EventsMappingDTO data)
        {
            return _interface.deactivate(data);
        }

        [Route("get_modeldata")]
        public EventsMappingDTO get_modeldata([FromBody]EventsMappingDTO data)
        {
            return _interface.get_modeldata(data);
        }

        [Route("Deactivesponsor")]
        public EventsMappingDTO Deactivesponsor([FromBody]EventsMappingDTO data)
        {
            return _interface.Deactivesponsor(data);
        }
        

    }
}
