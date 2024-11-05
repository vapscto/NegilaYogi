using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using CommonLibrary;
using System.Net.Http;
using SportsServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SportMasterHouseDessignationFacade : Controller
    {
        public SportMasterHouseDessignationInterface _masterhouse;

        public SportMasterHouseDessignationFacade(SportMasterHouseDessignationInterface masterhouse)
        {
            _masterhouse = masterhouse;
        }



        [Route("Getdetails/")]
        public SPCC_Master_House_Designation_DTO Getdetails([FromBody]SPCC_Master_House_Designation_DTO SPCC_Master_House_Designation_DTO)//int IVRMM_Id
        {

            return _masterhouse.GetmastercasteData(SPCC_Master_House_Designation_DTO);

        }

        [Route("GetSelectedRowDetails/{id:int}")]
        public SPCC_Master_House_Designation_DTO GetSelectedRowDetails(int ID)
        {

            return _masterhouse.GetSelectedRowDetails(ID);
        }

        [HttpPost]
        public SPCC_Master_House_Designation_DTO Post([FromBody] SPCC_Master_House_Designation_DTO masterMDT)
        {

            return _masterhouse.mastercasteData(masterMDT);
        }

        [Route("deactivate")]
        public SPCC_Master_House_Designation_DTO deactivate([FromBody]SPCC_Master_House_Designation_DTO dto)
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
