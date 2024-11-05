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
    public class DisposeAssetsFacadeController : Controller
    {
        // GET: api/values
        DisposeAssetsInterface _AT;
        public DisposeAssetsFacadeController(DisposeAssetsInterface AT)
        {
            _AT = AT;
        }
        [Route("getloaddata")]
        public DisposeAssetsDTO getloaddata([FromBody] DisposeAssetsDTO data)
        {
            return _AT.getloaddata(data);
        }
        [Route("getlocations")]
        public DisposeAssetsDTO getlocations([FromBody] DisposeAssetsDTO data)
        {
            return _AT.getlocations(data);
        }
        [Route("getitems")]
        public DisposeAssetsDTO getitems([FromBody] DisposeAssetsDTO data)
        {
            return _AT.getitems(data);
        }
        [Route("getdetails")]
        public DisposeAssetsDTO getdetails([FromBody] DisposeAssetsDTO data)
        {
            return _AT.getdetails(data);
        }
        
        [Route("savedetails")]
        public DisposeAssetsDTO savedetails([FromBody] DisposeAssetsDTO data)
        {
            return _AT.savedetails(data);
        }
        [Route("deactive")]
        public DisposeAssetsDTO deactive([FromBody] DisposeAssetsDTO data)
        {
            return _AT.deactive(data);
        }
   
       




    }
}
