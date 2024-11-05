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
    public class AssetTagDisposeFacade : Controller
    {
        // GET: api/values
        AssetTagDisposeInterface _AT;
        public AssetTagDisposeFacade(AssetTagDisposeInterface AT)
        {
            _AT = AT;
        }
        [Route("getloaddata")]
        public AssetTagDisposeDTO getloaddata([FromBody] AssetTagDisposeDTO data)
        {
            return _AT.getloaddata(data);
        }

        [Route("getlocation")]
        public AssetTagDisposeDTO getlocation([FromBody] AssetTagDisposeDTO data)
        {
            return _AT.getlocation(data);
        }
        [Route("getitems")]
        public AssetTagDisposeDTO getitems([FromBody] AssetTagDisposeDTO data)
        {
            return _AT.getitems(data);
        }
        [Route("getitemtagdata")]
        public AssetTagDisposeDTO getitemtagdata([FromBody] AssetTagDisposeDTO data)
        {
            return _AT.getitemtagdata(data);
        }
        [Route("savedata")]
        public AssetTagDisposeDTO savedata([FromBody] AssetTagDisposeDTO data)
        {
            return _AT.savedata(data);
        }
        [Route("deactive")]
        public AssetTagDisposeDTO deactive([FromBody] AssetTagDisposeDTO data)
        {
            return _AT.deactive(data);
        }

    }
}
