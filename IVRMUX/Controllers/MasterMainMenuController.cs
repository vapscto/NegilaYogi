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
    public class MasterMainMenuController : Controller
    {
        // GET: /<controller>/
        MasterMainMenuDelegates MasterMainMenudelStr = new MasterMainMenuDelegates();

        //// GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("Getdetails/")]
        public MasterMainMenuDTO Getdetails(MasterMainMenuDTO MasterMainMenuDTO)
        {

            return MasterMainMenudelStr.GetMasterMainMenuData(MasterMainMenuDTO);
            
        }

        [Route("GetSelectedRowdetails/{id:int}")]
        public MasterMainMenuDTO GetSelectedRowDetails(int ID)
        {
            return MasterMainMenudelStr.GetSelectedRowDetails(ID);
        }


        [HttpPost]
        public MasterMainMenuDTO MasterMainMenuDTO([FromBody] MasterMainMenuDTO MMD)
        {
            return MasterMainMenudelStr.MasterMainMenuDTO(MMD);
        }


        [HttpDelete]
        [Route("MasterDeleteMainMenuDTO/{id:int}")]
        public MasterMainMenuDTO MasterDeleteMainMenuDTO(int ID)
        {
            return MasterMainMenudelStr.MasterDeleteMainMenuDTO(ID);
        }




    }

}
