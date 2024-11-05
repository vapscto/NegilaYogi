using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Inventory.Sales;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

namespace IVRMUX.Controllers.com.vapstech.Inventory.Sales
{
    [Produces("application/json")]
    [Route("api/ISM_ClientProject_Mapping")]
    public class ISM_ClientProject_MappingController : Controller
    {
        ISM_ClientProject_MappingDelegate del = new ISM_ClientProject_MappingDelegate();
        [Route("loaddata/{id:int}")]
        public ISM_ClientProject_MappingDTO loaddata(int id)
        {
            ISM_ClientProject_MappingDTO data = new ISM_ClientProject_MappingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loaddata(data);
        }
        [Route("savedata")]
        public ISM_ClientProject_MappingDTO savedata([FromBody]ISM_ClientProject_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedata(data);
        }
        [Route("getproject")]
        public ISM_ClientProject_MappingDTO getproject([FromBody] ISM_ClientProject_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getproject(data);
        }
        [Route("getmodule")]
        public ISM_ClientProject_MappingDTO getmodule([FromBody] ISM_ClientProject_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getmodule(data);
        }
        [Route("EditData")]
        public ISM_ClientProject_MappingDTO Editdata([FromBody] ISM_ClientProject_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.Editdata(data);
        }
        [Route("clientDecative")]
        public ISM_ClientProject_MappingDTO clientDecative([FromBody]ISM_ClientProject_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.clientDecative(data);
        }
    }
}