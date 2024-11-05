using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using SportsServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SportMasterCompitionLevelFacade : Controller
    {
        public SportMasterCompitionLevelInterface _mastercompition;

        public SportMasterCompitionLevelFacade(SportMasterCompitionLevelInterface mastercompition)
        {
            _mastercompition = mastercompition;
        }



        [Route("Getdetails/")]
        public SportMasterCompitionLevelDTO Getdetails([FromBody]SportMasterCompitionLevelDTO SportMasterCompitionLevelDTO)//int IVRMM_Id
        {

            return _mastercompition.GetmastercasteData(SportMasterCompitionLevelDTO);

        }

        [Route("GetSelectedRowDetails/{id:int}")]
        public SportMasterCompitionLevelDTO GetSelectedRowDetails(int ID)
        {

            return _mastercompition.GetSelectedRowDetails(ID);
        }

        [HttpPost]
        public SportMasterCompitionLevelDTO Post([FromBody] SportMasterCompitionLevelDTO masterMDT)
        {

            return _mastercompition.mastercasteData(masterMDT);
        }

        [Route("deactivate")]
        public SportMasterCompitionLevelDTO deactivate([FromBody]SportMasterCompitionLevelDTO dto)
        {
            return _mastercompition.deactivate(dto);
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
