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
    public class CollegecastecategoryFacadeController : Controller
    {
        public CollegecastecategoryInterface _castecategory;

        public CollegecastecategoryFacadeController(CollegecastecategoryInterface castecategory)
        {
            _castecategory = castecategory;
        }

        [Route("Getdetails/")]
        public CollegecastecategoryDTO Getdetails([FromBody]CollegecastecategoryDTO CollegecastecategoryDTO)//int IVRMM_Id
        {

            return _castecategory.GetcastecategoryData(CollegecastecategoryDTO);

        }

        [Route("GetSelectedRowDetails/{id:int}")]
        public CollegecastecategoryDTO GetSelectedRowDetails(int ID)
        {

            return _castecategory.GetSelectedRowDetails(ID);
        }

        [HttpPost]
        public CollegecastecategoryDTO Post([FromBody] CollegecastecategoryDTO masterMDT)
        {

            return _castecategory.castecategoryData(masterMDT);
        }
       
        [Route("MasterDeleteModulesDATA/{id:int}")]
        public CollegecastecategoryDTO MasterDeleteModulesDATA(int ID)
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
