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
    public class MasterSportsCCNameFacade : Controller
    {
        MasterSportsCCNameInterface _interface;
        public MasterSportsCCNameFacade(MasterSportsCCNameInterface interfaces)
        {
            _interface = interfaces;
        }
        [Route("getDetails")]
        public MasterSportsCCNameDTO getDetails([FromBody]MasterSportsCCNameDTO data)
        {
            return _interface.getDetails(data);
        }
        [Route("save")]
        public MasterSportsCCNameDTO save([FromBody]MasterSportsCCNameDTO data)
        {
            return _interface.saveRecord(data);
        }
        [Route("EditDetails/{id:int}")]
        public MasterSportsCCNameDTO EditDetails(int id)
        {
            return _interface.EditDetails(id);
        }
        [Route("deactivate")]
        public MasterSportsCCNameDTO deactivate([FromBody]MasterSportsCCNameDTO data)
        {
            return _interface.deactivate(data);
        }
    }
}
