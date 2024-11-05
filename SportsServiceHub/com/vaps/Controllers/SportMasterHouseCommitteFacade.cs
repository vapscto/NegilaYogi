using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using SportsServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SportMasterHouseCommitteFacade : Controller
    {
        public SportMasterHouseCommitteInterface _masterhouse;

        public SportMasterHouseCommitteFacade(SportMasterHouseCommitteInterface masterhouse)
        {
            _masterhouse = masterhouse;
        }
       
        [Route("Getdetails")]
        public HouseCommitte_DTO Getdetails([FromBody]HouseCommitte_DTO HouseCommitte_DTO)//int IVRMM_Id
        {

            return _masterhouse.GetmastercasteData(HouseCommitte_DTO);

        }
        [Route("get_section/")]
        public HouseCommitte_DTO get_section([FromBody]HouseCommitte_DTO HouseCommitte_DTO)//int IVRMM_Id
        {

            return _masterhouse.get_section(HouseCommitte_DTO);

        }
        [Route("get_student/")]
        public HouseCommitte_DTO get_student([FromBody]HouseCommitte_DTO HouseCommitte_DTO)//int IVRMM_Id
        {

            return _masterhouse.get_student(HouseCommitte_DTO);

        }

        [Route("GetSelectedRowDetails/")]
        public HouseCommitte_DTO GetSelectedRowDetails([FromBody]HouseCommitte_DTO HouseCommitte_DTO)
        {

            return _masterhouse.GetSelectedRowDetails(HouseCommitte_DTO);
        }

        [HttpPost]
        public HouseCommitte_DTO Post([FromBody] HouseCommitte_DTO masterMDT)
        {

            return _masterhouse.mastercasteData(masterMDT);
        }

        [Route("deactivate")]
        public HouseCommitte_DTO deactivate([FromBody]HouseCommitte_DTO dto)
        {
            return _masterhouse.deactivate(dto);
        }

        [Route("onhousechage")]
        public HouseCommitte_DTO onhousechage([FromBody]HouseCommitte_DTO dto)
        {

            return _masterhouse.onhousechage(dto);
        }

        [Route("get_House")]
        public HouseCommitte_DTO get_House([FromBody]HouseCommitte_DTO dto)
        {

            return _masterhouse.get_House(dto);
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
