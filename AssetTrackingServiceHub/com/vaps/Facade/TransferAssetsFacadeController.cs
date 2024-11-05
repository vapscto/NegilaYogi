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
    public class TransferAssetsFacadeController : Controller
    {
        // GET: api/values
        TransferAssetsInterface _AT;
        public TransferAssetsFacadeController(TransferAssetsInterface AT)
        {
            _AT = AT;
        }
        [Route("getloaddata")]
        public Task<TransferAssetsDTO> getloaddata([FromBody] TransferAssetsDTO data)
        {
            return _AT.getloaddata(data);
        }
        [Route("gettolocations")]
        public TransferAssetsDTO gettolocations([FromBody] TransferAssetsDTO data)
        {
            return _AT.gettolocations(data);
        }
        [Route("getitemdetails")]
        public TransferAssetsDTO getitemdetails([FromBody] TransferAssetsDTO data)
        {
            return _AT.getitemdetails(data);
        }


        [Route("savedetails")]
        public TransferAssetsDTO savedetails([FromBody] TransferAssetsDTO data)
        {
            return _AT.savedetails(data);
        }
        [Route("deactive")]
        public TransferAssetsDTO deactive([FromBody] TransferAssetsDTO data)
        {
            return _AT.deactive(data);
        }






    }
}
