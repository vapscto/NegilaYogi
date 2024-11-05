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
    public class MasterSponserFacade : Controller
    {
        MasterSponserInterface _interface;
        public MasterSponserFacade(MasterSponserInterface interfaces)
        {
            _interface = interfaces;
        }
        [Route("getDetails")]
        public MasterSponserDTO getDetails([FromBody]MasterSponserDTO data)
        {
            return _interface.getDetails(data);
        }
        [Route("save")]
        public MasterSponserDTO save([FromBody]MasterSponserDTO data)
        {
            return _interface.saveRecord(data);
        }
        [Route("EditDetails/{id:int}")]
        public MasterSponserDTO EditDetails(int id)
        {
            return _interface.EditDetails(id);
        }
        [Route("deactivateSponser")]
        public MasterSponserDTO deactivateSponser([FromBody]MasterSponserDTO data)
        {
            return _interface.deactivateSponser(data);
        }
        

    }
}
