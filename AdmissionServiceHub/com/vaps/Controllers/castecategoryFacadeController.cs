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

    public class castecategoryFacadeController : Controller
    {
        public castecategoryInterface _castecategory;

        public castecategoryFacadeController(castecategoryInterface castecategory)
        {
            _castecategory = castecategory;
        }

    
        [HttpGet]

        [Route("Getdetails/")]
        public castecategoryDTO Getdetails(castecategoryDTO castecategoryDTO)//int IVRMM_Id
        {
           
            return _castecategory.GetcastecategoryData(castecategoryDTO);
           
        }

        [Route("GetSelectedRowDetails/{id:int}")]
        public castecategoryDTO GetSelectedRowDetails(int ID)
        {
           
            return _castecategory.GetSelectedRowDetails(ID);
        }
    
        [HttpPost]
        public castecategoryDTO Post([FromBody] castecategoryDTO masterMDT)
        {
          
            return _castecategory.castecategoryData(masterMDT);
        }

        [HttpDelete]
        [Route("MasterDeleteModulesDATA/{id:int}")]
        public castecategoryDTO MasterDeleteModulesDATA(int ID)
        {
           
            return _castecategory.MasterDeleteModulesData(ID);
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
