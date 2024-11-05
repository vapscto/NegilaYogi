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
    public class MasterSportsCCNameUOM_Facade : Controller
    {
        MasterSportsCCNameUOMInterface _interface;
        public MasterSportsCCNameUOM_Facade(MasterSportsCCNameUOMInterface interfaces)
        {
            _interface = interfaces;
        }
        [Route("getDetails")]
        public MasterSportsCCNameUMO_DTO getDetails([FromBody]MasterSportsCCNameUMO_DTO data)
        {
            return _interface.getDetails(data);
        }
        [Route("save")]
        public MasterSportsCCNameUMO_DTO save([FromBody]MasterSportsCCNameUMO_DTO data)
        {
            return _interface.saveRecord(data);
        }
        [Route("EditDetails/{id:int}")]
        public MasterSportsCCNameUMO_DTO EditDetails(int id)
        {
            return _interface.EditDetails(id);
        }
        [Route("deactivate")]
        public MasterSportsCCNameUMO_DTO deactivate([FromBody]MasterSportsCCNameUMO_DTO data)
        {
            return _interface.deactivate(data);
        }
    }
}
