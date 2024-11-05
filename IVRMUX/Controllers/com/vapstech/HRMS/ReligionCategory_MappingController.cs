using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.HRMS
{
    [Route("api/[controller]")]
    public class ReligionCategory_MappingController : Controller
    {
        ReligionCategory_MappingDelegate del = new ReligionCategory_MappingDelegate();
        // GET: api/<controller>
        [Route("loaddata/{id:int}")]
        public ReligionCategory_MappingDTO loaddata(int id)
        {
            ReligionCategory_MappingDTO data =new ReligionCategory_MappingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }
        [Route("savedata")]
        public ReligionCategory_MappingDTO savedata([FromBody] ReligionCategory_MappingDTO data)
        {           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedata(data);
        }
        [Route("Editdata")]
        public ReligionCategory_MappingDTO Editdata([FromBody] ReligionCategory_MappingDTO data)
        {           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.Editdata(data);
        }
        [Route("masterDecative")]
        public ReligionCategory_MappingDTO masterDecative([FromBody] ReligionCategory_MappingDTO data)
        {           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.masterDecative(data);
        }
        //[Route("getcast")]
        //public ReligionCategory_MappingDTO getcast([FromBody] ReligionCategory_MappingDTO data)
        //{
        //    data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return del.getcast(data);
        //}

    }
}
