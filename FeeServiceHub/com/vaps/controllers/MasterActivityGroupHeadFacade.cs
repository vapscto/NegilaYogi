using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model.com.vapstech.Fee;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class MasterActivityGroupHeadFacade : Controller
    {
        public MasterActivityGroupHeadInterface inter;
        public MasterActivityGroupHeadFacade(MasterActivityGroupHeadInterface s)
        {
            inter = s;
        }

        // GET: api/<controller>**/*//////////////////////////////////////////////////////
        [Route("loaddata")]
        public Adm_Master_ActivitiesDTO loaddata([FromBody] Adm_Master_ActivitiesDTO data)
        {
            return inter.loaddata(data);
        }

        [Route("gethead")]
        public Adm_Master_ActivitiesDTO gethead([FromBody] Adm_Master_ActivitiesDTO data)
        {
            return inter.gethead(data);
        }
        [Route("savedata")]
        public Adm_Master_ActivitiesDTO savedata([FromBody] Adm_Master_ActivitiesDTO data)
        {
            return inter.savedata(data);
        }
        [Route("masterDecative")]
        public Adm_Master_ActivitiesDTO masterDecative([FromBody]Adm_Master_ActivitiesDTO data)
        {
            return inter.masterDecative(data);
        }
        [Route("deletedata")]
        public Adm_Master_ActivitiesDTO deletedata([FromBody]Adm_Master_ActivitiesDTO data)
        {
            return inter.deletedata(data);
        }
    }
}
