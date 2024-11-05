using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model.com.vapstech.Fee;
using IVRMUX.Delegates.com.vapstech.Fees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    public class MasterActivityGroupHeadController : Controller
    {
        MasterActivityGroupHeadDelegate del = new MasterActivityGroupHeadDelegate();

        // GET: api/<controller>
        [Route("loaddata/{id:int}")]
        public Adm_Master_ActivitiesDTO loaddata(int id)
        {
            Adm_Master_ActivitiesDTO data = new Adm_Master_ActivitiesDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }

        [Route("gethead")]
        public Adm_Master_ActivitiesDTO gethead([FromBody] Adm_Master_ActivitiesDTO data)
        {
           // Adm_Master_ActivitiesDTO data = new Adm_Master_ActivitiesDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.gethead(data);
        }
        [Route("savedata")]
        public Adm_Master_ActivitiesDTO savedata([FromBody] Adm_Master_ActivitiesDTO data)
        {
           // Adm_Master_ActivitiesDTO data = new Adm_Master_ActivitiesDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedata(data);
         }
        [Route("deletedata")]
        public Adm_Master_ActivitiesDTO deletedata([FromBody] Adm_Master_ActivitiesDTO data)
        {
            // Adm_Master_ActivitiesDTO data = new Adm_Master_ActivitiesDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deletedata(data);
        }

        [Route("masterDecative")]
        public Adm_Master_ActivitiesDTO masterDecative([FromBody]Adm_Master_ActivitiesDTO data)
        {
           // Adm_Master_ActivitiesDTO data = new Adm_Master_ActivitiesDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.masterDecative(data);
        }
}
}
