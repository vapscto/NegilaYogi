using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Sport;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Sport;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Sport
{
    [Route("api/[controller]")]
    public class SportMasterDivisionController : Controller
    {
        SportMasterDivisionDelegate mastercastedelStr = new SportMasterDivisionDelegate();


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails/")]
        public SportMasterDivisionDTO Getdetails(SportMasterDivisionDTO SportMasterDivisionDTO)
        {
            SportMasterDivisionDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return mastercastedelStr.GetmastercasteData(SportMasterDivisionDTO);
        }

        [Route("GetSelectedRowdetails/{id:int}")]
        public SportMasterDivisionDTO GetSelectedRowDetails(int ID)
        {
            return mastercastedelStr.GetSelectedRowDetails(ID);
        }


        [HttpPost]
        public SportMasterDivisionDTO SportMasterDivisionDTO([FromBody] SportMasterDivisionDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastercastedelStr.mastercasteData(MMD);
        }

        [Route("deactivate")]
        public SportMasterDivisionDTO deactivate([FromBody] SportMasterDivisionDTO rel)
        {
            return mastercastedelStr.deactivate(rel);
        }
    }
}
