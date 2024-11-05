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
    public class AssetTagCheckInFacade : Controller
    {
        // GET: api/values
        AssetTagCheckInInterface _AT;
        public AssetTagCheckInFacade(AssetTagCheckInInterface AT)
        {
            _AT = AT;
        }
        [Route("getloaddata")]
        public AssetTagCheckInDTO getloaddata([FromBody] AssetTagCheckInDTO data)
        {
            return _AT.getloaddata(data);
        }
        
             [Route("getstore")]
        public AssetTagCheckInDTO getstore([FromBody] AssetTagCheckInDTO data)
        {
            return _AT.getstore(data);
        }
        [Route("getitems")]
        public AssetTagCheckInDTO getitems([FromBody] AssetTagCheckInDTO data)
        {
            return _AT.getitems(data);
        }
        [Route("getitemtagdata")]
        public AssetTagCheckInDTO getitemtagdata([FromBody] AssetTagCheckInDTO data)
        {
            return _AT.getitemtagdata(data);
        }
        [Route("savedata")]
        public AssetTagCheckInDTO savedata([FromBody] AssetTagCheckInDTO data)
        {
            return _AT.savedata(data);
        }
        [Route("deactive")]
        public AssetTagCheckInDTO deactive([FromBody] AssetTagCheckInDTO data)
        {
            return _AT.deactive(data);
        }

    }
}
