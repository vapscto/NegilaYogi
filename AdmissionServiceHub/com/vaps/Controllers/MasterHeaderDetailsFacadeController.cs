using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using AdmissionServiceHub.com.vaps.Services;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vapstech.Controllers
{
    [Route("api/[controller]")]

    public class MasterHeaderDetailsFacadeController : Controller
    {
        public MasterHeaderDetailsInterface _MasterHeaderDetailsInterface;


        public MasterHeaderDetailsFacadeController(MasterHeaderDetailsInterface MasterHeaderDetailsInterface)
        {
            _MasterHeaderDetailsInterface = MasterHeaderDetailsInterface;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
       // [HttpGet]

        [Route("Getdetails")]
        public MasterHeaderDetailsDTO Getdetails([FromBody]MasterHeaderDetailsDTO MasterHeaderDetailsDTO)//int IVRMM_Id
        {
            return _MasterHeaderDetailsInterface.GetMasterHeaderDetailsData(MasterHeaderDetailsDTO);
        }

      

        [HttpPost]
        [Route("SaveData/")]
        public MasterHeaderDetailsDTO SaveData([FromBody] MasterHeaderDetailsDTO masterMDT)
        {
            // return _reg.getregdata(reg);
            return _MasterHeaderDetailsInterface.SaveData(masterMDT);
        }


        [Route("GetSelectedRowDetails/")]
        public MasterHeaderDetailsDTO GetSelectedRowDetails([FromBody] MasterHeaderDetailsDTO ID)
        {
             
            return _MasterHeaderDetailsInterface.GetSelectedRowDetails(ID);
        }
        [Route("getmodulePage/")]
        public MasterHeaderDetailsDTO getmodulePage([FromBody] MasterHeaderDetailsDTO ID)
        {
            return _MasterHeaderDetailsInterface.getmodulePage(ID);
        }


        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("DeleteEntry/{id:int}")]
        public MasterHeaderDetailsDTO DeleteEntry(int ID)
        {
            // return _reg.getregdata(reg);
            return _MasterHeaderDetailsInterface.DeleteEntry(ID);
        }

    }
}
