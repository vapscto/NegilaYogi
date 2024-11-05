using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssetTrackingServiceHub.com.vaps.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AssetTrackingServiceHub.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class Assets_Expiredate_Facade : Controller
    {
        Assets_Expiredate_Interface _AEI;
        public Assets_Expiredate_Facade(Assets_Expiredate_Interface aei)
        {
            _AEI = aei;
        }

       [HttpPost]
       [Route("get_expdata")]

       public Asset_Expiredate_DTO get_expdata([FromBody]Asset_Expiredate_DTO dto)
        {
            
            return _AEI.get_expdata(dto);
        }

    }
}
