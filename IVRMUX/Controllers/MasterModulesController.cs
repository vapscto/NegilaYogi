using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model;
using PreadmissionDTOs;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterModulesController : Controller
    {
        // GET: /<controller>/
        MasterModulesDelegates MasterModulesdelStr = new MasterModulesDelegates();

        //// GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("Getdetails/")]
        public MasterModulesDTO Getdetails(MasterModulesDTO MasterModulesDTO)
        {
            return  MasterModulesdelStr.GetMasterModulesData(MasterModulesDTO);
            
        }

        [Route("GetSelectedRowdetails/{id:int}")]
        public MasterModulesDTO GetSelectedRowDetails(int ID)
        {
            return MasterModulesdelStr.GetSelectedRowDetails(ID);
        }


        [HttpPost]
        // public IActionResult Post([FromBody] regis reg)
        public MasterModulesDTO MasterModulesDTO([FromBody] MasterModulesDTO MMD)
        {
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            MMD.userid = UserId;
            return MasterModulesdelStr.MasterModulesData(MMD);
        }


        [HttpDelete]
        [Route("MasterDeleteModulesDTO/{id:int}")]
        public MasterModulesDTO MasterDeleteModulesDTO(int ID)
        {
            return MasterModulesdelStr.MasterDeleteModulesData(ID);
        }
    }

}
