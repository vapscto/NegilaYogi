using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Sports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class SportsMasterHouseController : Controller
    {
        // GET: api/<controller>
         SportsMasterHouseDelegate _delobj = new SportsMasterHouseDelegate();

        [Route("getdetails/{id:int}")]
        public SportsMasterHouse_DTO getdetails(int a)
        {
            SportsMasterHouse_DTO SportsMasterHouse_DTO = new SportsMasterHouse_DTO();
            SportsMasterHouse_DTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _delobj.getdetails(SportsMasterHouse_DTO);
        }      


        [Route("savedata")]
        public SportsMasterHouse_DTO savedata([FromBody] SportsMasterHouse_DTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.savedata(MMD);
        }

        [Route("deactivate")]
        public SportsMasterHouse_DTO deactivate([FromBody] SportsMasterHouse_DTO rel)
        {
            rel.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.deactivate(rel);
        }

        [Route("editdata")]
        public SportsMasterHouse_DTO editdata(SportsMasterHouse_DTO data)
        {
          
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _delobj.editdata(data);
        }

    }
}
