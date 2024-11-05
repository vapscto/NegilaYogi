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
    public class MasterSubMenuController : Controller
    {
        // GET: /<controller>/
        MasterSubMenuDelegates MasterSubMenudelStr = new MasterSubMenuDelegates();

        //// GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("Getdetails/")]
        public MasterMainMenuDTO Getdetails(MasterMainMenuDTO MasterMainMenuDTO)
        {

            return MasterSubMenudelStr.GetMasterSubMenuData(MasterMainMenuDTO);
            
        }

        [Route("GetSelectedRowdetails/{id:int}")]
        public MasterMainMenuDTO GetSelectedRowDetails(int ID)
        {
            return MasterSubMenudelStr.GetSelectedRowDetails(ID);
        }


        [HttpPost]
        public MasterMainMenuDTO MasterMainMenuDTO([FromBody] MasterMainMenuDTO MMD)
        {
            return MasterSubMenudelStr.MasterMainMenuDTO(MMD);
        }


        [HttpDelete]
        [Route("MasterDeleteSubMenuDTO/{id:int}")]
        public MasterMainMenuDTO MasterDeleteSubMenuDTO(int ID)
        {
            return MasterSubMenudelStr.MasterDeleteSubMenuDTO(ID);
        }




    }

}
