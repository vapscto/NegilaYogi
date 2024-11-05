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

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterActivityController : Controller
    {
       
        MasterActivityDelegates MasterActivitydelStr = new MasterActivityDelegates();

      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails/")]
        public MasterActivityDTO Getdetails(MasterActivityDTO MasterActivityDTO)
        {           

            return MasterActivitydelStr.GetMasterActivityData(MasterActivityDTO);            
        }

        [Route("GetSelectedRowdetails/{id:int}")]
        public MasterActivityDTO GetSelectedRowDetails(int ID)
        {
            HttpContext.Session.SetString("MasterActivityID", ID.ToString());
            return MasterActivitydelStr.GetSelectedRowDetails(ID);
        }


        [HttpPost]      
        public MasterActivityDTO MasterActivityDTO([FromBody] MasterActivityDTO MMD)
        {
            Int32 MasterActivityID = 0;
            if (HttpContext.Session.GetString("MasterActivityID") != null)
            {
                MasterActivityID = Convert.ToInt32(HttpContext.Session.GetString("MasterActivityID"));
            }
            MMD.AMA_Id = MasterActivityID;
            HttpContext.Session.Remove("MasterActivityID");

            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return MasterActivitydelStr.MasterActivityData(MMD);         
           // return MMD;           
        }

        [HttpGet]
        [Route("MasterDeleteModulesDTO/{id:int}")]
        public MasterActivityDTO MasterDeleteModulesDTO(int ID)
        {
            return MasterActivitydelStr.MasterDeleteModulesData(ID);         
        }
    }

}
