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
    public class castecategoryController : Controller
    {
        castecategoryDelegates castecategorydelStr = new castecategoryDelegates();
              
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Getdetails/")]
        public castecategoryDTO Getdetails(castecategoryDTO castecategoryDTO)
        {
            return castecategorydelStr.GetcastecategoryData(castecategoryDTO);            
        }

        [Route("GetSelectedRowdetails/{id:int}")]
        public castecategoryDTO GetSelectedRowDetails(int ID)
        {
            return castecategorydelStr.GetSelectedRowDetails(ID);
        }

        [HttpPost]      
        public castecategoryDTO castecategoryDTO([FromBody] castecategoryDTO MMD)
        {
            return castecategorydelStr.castecategoryData(MMD);         
        }
        
        [Route("MasterDeleteModulesDTO/{id:int}")]
        public castecategoryDTO castecategoryDTO(int ID)
        {
            return castecategorydelStr.MasterDeleteModulesData(ID);         
        }
    }

}
