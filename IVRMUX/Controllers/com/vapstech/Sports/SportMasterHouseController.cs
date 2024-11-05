using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Sports;
using corewebapi18072016.Delegates.com.vapstech.Sports;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class SportMasterHouseController : Controller
    {
        SportMasterHouseDelegate mastercastedelStr = new SportMasterHouseDelegate();


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails/")]
        public SportMasterHouseDTO Getdetails(SportMasterHouseDTO SportMasterHouseDTO)
        {
            SportMasterHouseDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return mastercastedelStr.GetmastercasteData(SportMasterHouseDTO);
        }

        [Route("GetSelectedRowdetails/{id:int}")]
        public SportMasterHouseDTO GetSelectedRowDetails(int ID)
        {
            return mastercastedelStr.GetSelectedRowDetails(ID);
        }


        [HttpPost]
        public SportMasterHouseDTO SportMasterHouseDTO([FromBody] SportMasterHouseDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastercastedelStr.mastercasteData(MMD);
        }

        [Route("deactivate")]
        public SportMasterHouseDTO deactivate([FromBody] SportMasterHouseDTO rel)
        {
            return mastercastedelStr.deactivate(rel);
        }
    }
}
