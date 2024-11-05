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

    public class MasterPeriodFacadeController : Controller
    {
        public MasterPeriodInterface _MasterPeriodInterface;


        public MasterPeriodFacadeController(MasterPeriodInterface MasterPeriodInterface)
        {
            _MasterPeriodInterface = MasterPeriodInterface;
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
        public MasterPeriodDTO Getdetails([FromBody]MasterPeriodDTO MasterPeriodDTO)//int IVRMM_Id
        {
            return _MasterPeriodInterface.GetMasterPeriodData(MasterPeriodDTO);
        }

      

        [HttpPost]
        public MasterPeriodDTO Post([FromBody] MasterPeriodDTO masterMDT)
        {
            // return _reg.getregdata(reg);
            return _MasterPeriodInterface.SaveData(masterMDT);
        }


        [Route("GetSelectedRowDetails/")]
        public MasterPeriodDTO GetSelectedRowDetails([FromBody] MasterPeriodDTO ID)
        {
            // return _reg.getregdata(reg);      
            return _MasterPeriodInterface.GetSelectedRowDetails(ID);
        }


        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/values/5
        [HttpGet]
        [Route("DeleteEntry/{id:int}")]
        public MasterPeriodDTO DeleteEntry(int ID)
        {
            // return _reg.getregdata(reg);
            return _MasterPeriodInterface.DeleteEntry(ID);
        }

    }
}
