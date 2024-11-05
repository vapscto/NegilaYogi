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
    public class AssetTagCheckOutFacade : Controller
    {
        // GET: api/values
        AssetTagCheckOutInterface _AT;
        public AssetTagCheckOutFacade(AssetTagCheckOutInterface AT)
        {
            _AT = AT;
        }
        [Route("getloaddata")]
        public AssetTagCheckOutDTO getloaddata([FromBody] AssetTagCheckOutDTO data)
        {
            return _AT.getloaddata(data);
        }

        [Route("getitems")]
        public AssetTagCheckOutDTO getitems([FromBody] AssetTagCheckOutDTO data)
        {
            return _AT.getitems(data);
        }
        [Route("getitemtagdata")]
        public Task<AssetTagCheckOutDTO> getitemtagdata([FromBody] AssetTagCheckOutDTO data)
        {
            return _AT.getitemtagdata(data);
        }
        [Route("savedata")]
        public AssetTagCheckOutDTO savedata([FromBody] AssetTagCheckOutDTO data)
        {
            return _AT.savedata(data);
        }
        [Route("deactive")]
        public AssetTagCheckOutDTO deactive([FromBody] AssetTagCheckOutDTO data)
        {
            return _AT.deactive(data);
        }

    }
}
