using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking;
using AssetTrackingServiceHub.com.vaps.Interface;
using AssetTrackingServiceHub.com.vaps.AssetTag.Interface;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Controllers
{
    [Route("api/[controller]")]
    public class AssetTagFacade : Controller
    {
        // GET: api/values
        AssetTagInterface _AT;
        public AssetTagFacade(AssetTagInterface AT)
        {
            _AT = AT;
        }
        [Route("getloaddata")]
        public AssetTagDTO getloaddata([FromBody] AssetTagDTO data)
        {
            return _AT.getloaddata(data);
        }
        [Route("getdata")]
        public Task<AssetTagDTO> getdata([FromBody] AssetTagDTO data)
        {
            return _AT.getdata(data);
        }
        [Route("savedata")]
        public AssetTagDTO savedata([FromBody] AssetTagDTO data)
        {
            return _AT.savedata(data);
        }
        [Route("deactive")]
        public AssetTagDTO deactive([FromBody] AssetTagDTO data)
        {
            return _AT.deactive(data);
        }
        
    }
}
