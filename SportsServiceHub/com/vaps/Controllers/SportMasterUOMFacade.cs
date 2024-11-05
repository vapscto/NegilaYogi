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
    public class SportMasterUOMFacade : Controller
    {
        public SportMasterUOMInterface _masterhouse;

        public SportMasterUOMFacade(SportMasterUOMInterface masterhouse)
        {
            _masterhouse = masterhouse;
        }



        [Route("Getdetails/")]
        public SportMasterUOMDTO Getdetails([FromBody]SportMasterUOMDTO SportMasterUOMDTO)//int IVRMM_Id
        {

            return _masterhouse.GetmastercasteData(SportMasterUOMDTO);

        }

        [Route("GetSelectedRowDetails/{id:int}")]
        public SportMasterUOMDTO GetSelectedRowDetails(int ID)
        {

            return _masterhouse.GetSelectedRowDetails(ID);
        }

        [HttpPost]
        public SportMasterUOMDTO Post([FromBody] SportMasterUOMDTO masterMDT)
        {

            return _masterhouse.mastercasteData(masterMDT);
        }

        [Route("deactivate")]
        public SportMasterUOMDTO deactivate([FromBody]SportMasterUOMDTO dto)
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
