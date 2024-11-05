using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model.com.vaps.admission;
using PreadmissionDTOs.com.vaps.admission;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;


namespace corewebapi18072016.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class GovernmentBondController : Controller 
    {
        
        GovernmentBondDelegates GovernmentBonddelStr = new GovernmentBondDelegates();


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails/")]
        public GovernmentBondDTO Getdetails(GovernmentBondDTO GovernmentBondDTO)
        {
            GovernmentBondDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return GovernmentBonddelStr.GetGovernmentBondData(GovernmentBondDTO);
        }

        [Route("GetSelectedRowdetails/{id:int}")]
        public GovernmentBondDTO GetSelectedRowDetails(int ID)
        {
            return GovernmentBonddelStr.GetSelectedRowDetails(ID);
        }


        [HttpPost]
        public GovernmentBondDTO GovernmentBondDTO([FromBody] GovernmentBondDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return GovernmentBonddelStr.GovernmentBondData(MMD);
        }

       
        [Route("MasterDeleteModulesDTO/{id:int}")]
        public GovernmentBondDTO MasterDeleteModulesDTO(int id)
        {
            GovernmentBondDTO dto = new GovernmentBondDTO();
            dto.IMGB_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return GovernmentBonddelStr.MasterDeleteModulesData(dto);
        }
    }
}
