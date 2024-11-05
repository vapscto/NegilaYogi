using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking;
using AssetTrackingServiceHub.com.vaps.Interface;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AssetTrackingServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class CheckInAssetsFacadeController : Controller
    {
        // GET: api/values
        CheckInAssetsInterface _AT;
        public CheckInAssetsFacadeController(CheckInAssetsInterface AT)
        {
            _AT = AT;
        }
        [Route("getloaddata")]
        public CheckInAssetsDTO getloaddata([FromBody] CheckInAssetsDTO data)
        {
            return _AT.getloaddata(data);
        }
        [Route("getStore")]
        public CheckInAssetsDTO getStore([FromBody] CheckInAssetsDTO data)
        {
            return _AT.getStore(data);
        }
        [Route("getitems")]
        public CheckInAssetsDTO getitems([FromBody] CheckInAssetsDTO data)
        {
            return _AT.getitems(data);
        }
        [Route("getdetails")]
        public CheckInAssetsDTO getdetails([FromBody] CheckInAssetsDTO data)
        {
            return _AT.getdetails(data);
        }
        
        [Route("savedetails")]
        public CheckInAssetsDTO savedetails([FromBody] CheckInAssetsDTO data)
        {
            return _AT.savedetails(data);
        }
        [Route("deactive")]
        public CheckInAssetsDTO deactive([FromBody] CheckInAssetsDTO data)
        {
            return _AT.deactive(data);
        }
   
     




    }
}
