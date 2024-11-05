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
    public class mappingFacade : Controller
    {
        public mappingInterface _cms;

        public mappingFacade(mappingInterface cmsdept)
        {
            _cms = cmsdept;
        }
        [HttpGet]
        [Route("loaddata/{id:int}")]
        public mappingDTO loaddata(int id)
        {
            return _cms.loaddata(id);
            // return _cms.loaddata(id);
        }
        //save
        [HttpPost]
        [Route("savedata")]
        public mappingDTO savedata([FromBody]mappingDTO data)
        {
            return _cms.savedata(data);
        }
        //edit
        [Route("edit")]
        public mappingDTO edit([FromBody]mappingDTO data)
        {
            return _cms.edit(data);
        }
        //deactive
        [Route("deactive")]
        public mappingDTO deactive([FromBody]mappingDTO data)
        {
            return _cms.deactive(data);
        }
    }
}
