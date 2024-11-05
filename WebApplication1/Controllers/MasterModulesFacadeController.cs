using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Services;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]

    public class MasterModulesFacadeController : Controller
    {
        public MasterModulesInterface _MasterModule;


        public MasterModulesFacadeController(MasterModulesInterface MasterModule)
        {
            _MasterModule = MasterModule;
        }

        // GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/values/5
        [HttpGet]

        [Route("Getdetails/")]
        public MasterModulesDTO Getdetails(MasterModulesDTO MasterModulesDTO)//int IVRMM_Id
        {
            // bool val = _MasterModule.GetMasterModulesData(IVRMM_Id);
            //  Array masterModulesname = _MasterModule.GetMasterModulesData(MasterModulesDTO);
            //reg.user_ip_address = Convert.ToString(val);
            return _MasterModule.GetMasterModulesData(MasterModulesDTO);
            //return "value";
        }

        [Route("GetSelectedRowDetails/{id:int}")]
        public MasterModulesDTO GetSelectedRowDetails(int ID)
        {
            // return _reg.getregdata(reg);      
            return _MasterModule.GetSelectedRowDetails(ID);
        }

        // POST api/values
        [HttpPost]
        public MasterModulesDTO Post([FromBody] MasterModulesDTO masterMDT)
        {
            // return _reg.getregdata(reg);
            return _MasterModule.MasterModulesData(masterMDT);
        }

        [HttpDelete]
        [Route("MasterDeleteModulesDATA/{id:int}")]
        public MasterModulesDTO MasterDeleteModulesDATA(int ID)
        {
            // return _reg.getregdata(reg);
            return _MasterModule.MasterDeleteModulesData(ID);
        }

        //[HttpPost]
        //public bool Postlogin([FromBody] regis reg)
        //{
        //    bool val = _reg.getregdata(reg);
        //    reg.user_ip_address = Convert.ToString(val);
        //    return val;
        //    // return _reg.regdata(reg);
        //}


        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }

    }
}
