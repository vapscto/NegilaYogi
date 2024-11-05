using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sport;
using SportServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SportServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SportMasterDivisionFacadeController : Controller
    {
        public SportMasterDivisionInterface _masterdivision;

        public SportMasterDivisionFacadeController(SportMasterDivisionInterface masterdivision)
        {
            _masterdivision = masterdivision;
        }



        [Route("Getdetails/")]
        public SportMasterDivisionDTO Getdetails([FromBody]SportMasterDivisionDTO SportMasterDivisionDTO)//int IVRMM_Id
        {

            return _masterdivision.GetmastercasteData(SportMasterDivisionDTO);

        }

        [Route("GetSelectedRowDetails/{id:int}")]
        public SportMasterDivisionDTO GetSelectedRowDetails(int ID)
        {

            return _masterdivision.GetSelectedRowDetails(ID);
        }

        [HttpPost]
        public SportMasterDivisionDTO Post([FromBody] SportMasterDivisionDTO masterMDT)
        {

            return _masterdivision.mastercasteData(masterMDT);
        }

        [Route("deactivate")]
        public SportMasterDivisionDTO deactivate([FromBody]SportMasterDivisionDTO dto)
        {
            return _masterdivision.deactivate(dto);
        }

        //[HttpDelete]
        //[Route("MasterDeleteModulesDATA/{id:int}")]
        //public SportMasterDivisionDTO MasterDeleteModulesDATA(int ID)
        //{

        //    return _masterdivision.MasterDeleteModulesData(ID);
        //}


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
