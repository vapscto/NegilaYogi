using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.TT;
using TimeTableServiceHub.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTableServiceHub.com.vaps.Controllers.College
{
    [Route("api/[controller]")]
    public class CLGMasterBuildingFacade : Controller
    {
       public CLGMasterBuildingInterface inter;

        public CLGMasterBuildingFacade(CLGMasterBuildingInterface abc)
        {
            inter = abc;
        }

        [Route("getdetails")]
       public CLGMasterBuilding_DTO getdetails([FromBody] CLGMasterBuilding_DTO data)
        {
            return inter.getdetails(data);
        }
        [Route("savedetail")]
        public CLGMasterBuilding_DTO savedetail([FromBody] CLGMasterBuilding_DTO data)
        {
            return inter.savedetail(data);
        }
      [Route("savedetail1")]
      public CLGMasterBuilding_DTO savedetail1([FromBody] CLGMasterBuilding_DTO data)
        {
            return inter.savedetail1(data);
        }





        [Route("getpagedetails1/{id:int}")]
        public CLGMasterBuilding_DTO getpagedetails1(int id)
        {
            return inter.getpagedetails1(id);
        }
        [Route("deactive1")]
        public CLGMasterBuilding_DTO deactive1([FromBody] CLGMasterBuilding_DTO id)
        {
            return inter.deactive1(id);
        }
    }
}
