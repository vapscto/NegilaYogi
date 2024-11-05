using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using SportsServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SportsMasterHouseFacade : Controller
    {
        // GET: api/<controller>
        SportsMasterHouseInterface _interface;
        public SportsMasterHouseFacade(SportsMasterHouseInterface interfaces)
        {
            _interface = interfaces;
        }

        [Route("getdetails")]
        public SportsMasterHouse_DTO getdetails([FromBody]SportsMasterHouse_DTO data)
        {
            return _interface.getdetails(data);
        }
        
        [Route("savedata")]
        public SportsMasterHouse_DTO savedata([FromBody]SportsMasterHouse_DTO data)
        {
            return _interface.savedata(data);
        }


        [Route("deactivate")]
        public SportsMasterHouse_DTO deactivate([FromBody]SportsMasterHouse_DTO data)
        {
            return _interface.deactivate(data);
        }

        [Route("editdata")]
        public SportsMasterHouse_DTO editdata([FromBody]SportsMasterHouse_DTO data)
        {
            return _interface.editdata(data);
        }

    }
}
