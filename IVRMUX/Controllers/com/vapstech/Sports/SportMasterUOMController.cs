using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Sports;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class SportMasterUOMController : Controller
    {
        SportMasterUOMDelegate mastercastedelStr = new SportMasterUOMDelegate();


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails/")]
        public SportMasterUOMDTO Getdetails(SportMasterUOMDTO SportMasterUOMDTO)
        {
            SportMasterUOMDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return mastercastedelStr.GetmastercasteData(SportMasterUOMDTO);
        }

        [Route("GetSelectedRowdetails/{id:int}")]
        public SportMasterUOMDTO GetSelectedRowDetails(int ID)
        {
            return mastercastedelStr.GetSelectedRowDetails(ID);
        }


        [HttpPost]
        public SportMasterUOMDTO SportMasterUOMDTO([FromBody] SportMasterUOMDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastercastedelStr.mastercasteData(MMD);
        }

        [Route("deactivate")]
        public SportMasterUOMDTO deactivate([FromBody] SportMasterUOMDTO rel)
        {
            return mastercastedelStr.deactivate(rel);
        }
    }
}
