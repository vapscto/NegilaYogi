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
    public class AssetTagTransferFacade : Controller
    {
        // GET: api/values
        AssetTagTransferInterface _AT;
        public AssetTagTransferFacade(AssetTagTransferInterface AT)
        {
            _AT = AT;
        }
        [Route("getloaddata")]
        public Task<AssetTagTransferDTO> getloaddata([FromBody] AssetTagTransferDTO data)
        {
            return _AT.getloaddata(data);
        }

        [Route("getitems")]
        public AssetTagTransferDTO getitems([FromBody] AssetTagTransferDTO data)
        {
            return _AT.getitems(data);
        }
        [Route("gettolocation")]
        public AssetTagTransferDTO gettolocation([FromBody] AssetTagTransferDTO data)
        {
            return _AT.gettolocation(data);
        }
        [Route("getitemtagdata")]
        public Task<AssetTagTransferDTO> getitemtagdata([FromBody] AssetTagTransferDTO data)
        {
            return _AT.getitemtagdata(data);
        }
        [Route("savedata")]
        public AssetTagTransferDTO savedata([FromBody] AssetTagTransferDTO data)
        {
            return _AT.savedata(data);
        }
        [Route("deactive")]
        public AssetTagTransferDTO deactive([FromBody] AssetTagTransferDTO data)
        {
            return _AT.deactive(data);
        }

    }
}
