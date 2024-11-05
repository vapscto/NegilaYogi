using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.admission;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    [Route("api/[controller]")]

    public class mastercasteFacadeController : Controller
    {
        public mastercasteInterface _mastercaste;

        public mastercasteFacadeController(mastercasteInterface mastercaste)
        {
            _mastercaste = mastercaste;
        }

    

        [Route("Getdetails/")]
        public mastercasteDTO Getdetails([FromBody]mastercasteDTO mastercasteDTO)//int IVRMM_Id
        {
           
            return _mastercaste.GetmastercasteData(mastercasteDTO);
           
        }

        [Route("GetSelectedRowDetails/{id:int}")]
        public mastercasteDTO GetSelectedRowDetails(int ID)
        {
           
            return _mastercaste.GetSelectedRowDetails(ID);
        }
    
        [HttpPost]
        public mastercasteDTO Post([FromBody] mastercasteDTO masterMDT)
        {
          
            return _mastercaste.mastercasteData(masterMDT);
        }

        [HttpGet]
        [Route("MasterDeleteModulesDATA/{id:int}")]
        public mastercasteDTO MasterDeleteModulesDATA(int ID)
        {
           
            return _mastercaste.MasterDeleteModulesData(ID);
        }

     
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
