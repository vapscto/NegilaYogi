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
    public class SportMasterHouseFacade : Controller
    {
        public SportMasterHouseInterface _masterhouse;

        public SportMasterHouseFacade(SportMasterHouseInterface masterhouse)
        {
            _masterhouse = masterhouse;
        }



        [Route("Getdetails/")]
        public SportMasterHouseDTO Getdetails([FromBody]SportMasterHouseDTO SportMasterHouseDTO)//int IVRMM_Id
        {

            return _masterhouse.GetmastercasteData(SportMasterHouseDTO);

        }

        [Route("GetSelectedRowDetails/{id:int}")]
        public SportMasterHouseDTO GetSelectedRowDetails(int ID)
        {

            return _masterhouse.GetSelectedRowDetails(ID);
        }

        [HttpPost]
        public SportMasterHouseDTO Post([FromBody] SportMasterHouseDTO masterMDT)
        {

            return _masterhouse.mastercasteData(masterMDT);
        }

        [Route("deactivate")]
        public SportMasterHouseDTO deactivate([FromBody]SportMasterHouseDTO dto)
        {
            return _masterhouse.deactivate(dto);
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
