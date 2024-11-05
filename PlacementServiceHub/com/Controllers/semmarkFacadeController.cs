using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlacementServiceHub.com.Interfaces;
using PreadmissionDTOs.com.vaps.Placement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlacementServiceHub.com.Controllers
{
    [Route("api/[controller]")]
    public class semmarkFacade : Controller
    {
        public semmarkInterface _cms;

        public semmarkFacade(semmarkInterface cmsdept)
        {
            _cms = cmsdept;
        }
        [HttpGet]
        [Route("loaddata/{id:int}")]
        public semmarkDTO loaddata(int id)
        {
            return _cms.loaddata(id);
            // return _cms.loaddata(id);
        }
        
        [Route("savedata")]
        public semmarkDTO savedata([FromBody]semmarkDTO data)
        {
            return _cms.savedata(data);
        }
        //edit
        [Route("edit")]
        public semmarkDTO edit([FromBody]semmarkDTO data)
        {
            return _cms.edit(data);
        }
        //deactive
        [Route("deactive")]
        public semmarkDTO deactive([FromBody]semmarkDTO data)
        {
            return _cms.deactive(data);
        }

    }
}
