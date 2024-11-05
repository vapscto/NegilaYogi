using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Placement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Placement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Placement
{
    [Route("api/[controller]")]
    public class mapping : Controller
    {
        mappingDelgate cms = new mappingDelgate();

        [HttpGet]
        [Route("loaddata/{id:int}")]
        public mappingDTO loaddata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return cms.loaddata(id);

        }
        //save
        [HttpPost]
        [Route("savedata")]
        public mappingDTO savedata([FromBody]mappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.savedata(data);
        }
        //edit
        [HttpPost]
        [Route("edit")]
        public mappingDTO edit([FromBody]mappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.edit(data);
        }
        //deactive
        [Route("deactive")]
        public mappingDTO deactive([FromBody]mappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.deactive(data);
        }
    }
}
