using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using corewebapi18072016.Delegates.com.vapstech.Sports;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class SportMasterCompitionLevelController : Controller
    {
        SportMasterCompitionLevelDelegate mastercastedelStr = new SportMasterCompitionLevelDelegate();


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails/")]
        public SportMasterCompitionLevelDTO Getdetails(SportMasterCompitionLevelDTO SportMasterCompitionLevelDTO)
        {
            SportMasterCompitionLevelDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return mastercastedelStr.GetmastercasteData(SportMasterCompitionLevelDTO);
        }

        [Route("GetSelectedRowdetails/{id:int}")]
        public SportMasterCompitionLevelDTO GetSelectedRowDetails(int ID)
        {
            return mastercastedelStr.GetSelectedRowDetails(ID);
        }


        [HttpPost]
        public SportMasterCompitionLevelDTO SportMasterCompitionLevelDTO([FromBody] SportMasterCompitionLevelDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastercastedelStr.mastercasteData(MMD);
        }

        [Route("deactivate")]
        public SportMasterCompitionLevelDTO deactivate([FromBody] SportMasterCompitionLevelDTO rel)
        {
            return mastercastedelStr.deactivate(rel);
        }
    }
}
