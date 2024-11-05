using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class CollegemastercasteFacadeController : Controller
    {
        public CollegemastercasteInterface _mastercaste;

        public CollegemastercasteFacadeController(CollegemastercasteInterface mastercaste)
        {
            _mastercaste = mastercaste;
        }


        [Route("Getdetails/")]
        public CollegemastercasteDTO Getdetails([FromBody]CollegemastercasteDTO mastercasteDTO)//int IVRMM_Id
        {
            return _mastercaste.GetmastercasteData(mastercasteDTO);
        }

        [Route("GetSelectedRowDetails/{id:int}")]
        public CollegemastercasteDTO GetSelectedRowDetails(int ID)
        {
            return _mastercaste.GetSelectedRowDetails(ID);
        }

        [HttpPost]
        public CollegemastercasteDTO Post([FromBody] CollegemastercasteDTO masterMDT)
        {
            return _mastercaste.mastercasteData(masterMDT);
        }
        
        [Route("MasterDeleteModulesDATA/{id:int}")]
        public CollegemastercasteDTO MasterDeleteModulesDATA(int ID)
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
