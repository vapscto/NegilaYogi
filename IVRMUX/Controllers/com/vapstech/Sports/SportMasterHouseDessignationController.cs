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
    public class SportMasterHouseDessignationController : Controller
    {
        SportMasterHouseDessignationDelegate mastercastedelStr = new SportMasterHouseDessignationDelegate();


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails/")]
        public SPCC_Master_House_Designation_DTO Getdetails(SPCC_Master_House_Designation_DTO SPCC_Master_House_Designation_DTO)
        {
            SPCC_Master_House_Designation_DTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return mastercastedelStr.GetmastercasteData(SPCC_Master_House_Designation_DTO);
        }

        [Route("GetSelectedRowdetails/{id:int}")]
        public SPCC_Master_House_Designation_DTO GetSelectedRowDetails(int ID)
        {
            return mastercastedelStr.GetSelectedRowDetails(ID);
        }


        [HttpPost]
        public SPCC_Master_House_Designation_DTO SPCC_Master_House_Designation_DTO([FromBody] SPCC_Master_House_Designation_DTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastercastedelStr.mastercasteData(MMD);
        }

        [Route("deactivate")]
        public SPCC_Master_House_Designation_DTO deactivate([FromBody] SPCC_Master_House_Designation_DTO rel)
        {
            return mastercastedelStr.deactivate(rel);
        }
    }
}
