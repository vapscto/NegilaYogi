using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.TT.College;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.TT.College
{
    [Route("api/[controller]")]
    public class CLGMasterBuildingController : Controller
    {
        CLGMasterBuildingDelegate objdelegate = new CLGMasterBuildingDelegate();
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public CLGMasterBuilding_DTO Get( int id)
        {
            CLGMasterBuilding_DTO data = new CLGMasterBuilding_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          
            return objdelegate.getdetails(data);
        } 
        [Route("savedetail")]
        public CLGMasterBuilding_DTO savedetail([FromBody] CLGMasterBuilding_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail(data);
        }
      [Route("savedetail1")]
      public CLGMasterBuilding_DTO savedetail1([FromBody] CLGMasterBuilding_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail1(data);
        }
        [Route("getpagedetails1/{id:int}")]
        public CLGMasterBuilding_DTO getpagedetails1(int id)
        {
            return objdelegate.getpagedetails1(id);
        }
        [Route("deactivate1")]
        public CLGMasterBuilding_DTO deactive1([FromBody]CLGMasterBuilding_DTO id)
        {
            return objdelegate.deactive1(id);
        }

    }
}
