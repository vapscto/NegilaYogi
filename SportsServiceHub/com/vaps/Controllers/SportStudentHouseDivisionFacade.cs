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
    public class SportStudentHouseDivisionFacade : Controller
    {
        public SportStudentHouseDivisionInterface _masterhouse;

        public SportStudentHouseDivisionFacade(SportStudentHouseDivisionInterface masterhouse)
        {
            _masterhouse = masterhouse;
        }

        [Route("Getdetails")]
        public SportMasterHouseDTO Getdetails([FromBody]SportMasterHouseDTO SportMasterHouseDTO)//int IVRMM_Id
        {

            return _masterhouse.GetmastercasteData(SportMasterHouseDTO);

        }
        [Route("get_section/")]
        public SportMasterHouseDTO get_section([FromBody]SportMasterHouseDTO SportMasterHouseDTO)//int IVRMM_Id
        {

            return _masterhouse.get_section(SportMasterHouseDTO);

        }
        [Route("get_student/")]
        public SportMasterHouseDTO get_student([FromBody]SportMasterHouseDTO SportMasterHouseDTO)//int IVRMM_Id
        {

            return _masterhouse.get_student(SportMasterHouseDTO);

        }

        [Route("GetSelectedRowDetails/")]
        public SportMasterHouseDTO GetSelectedRowDetails([FromBody]SportMasterHouseDTO SportMasterHouseDTO)
        {

            return _masterhouse.GetSelectedRowDetails(SportMasterHouseDTO);
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
