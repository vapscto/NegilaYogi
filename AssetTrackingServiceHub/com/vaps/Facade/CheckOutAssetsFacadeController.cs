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
    public class CheckOutAssetsFacadeController : Controller
    {
        // GET: api/values
        CheckOutAssetsInterface _AT;
        public CheckOutAssetsFacadeController(CheckOutAssetsInterface AT)
        {
            _AT = AT;
        }
        [Route("getloaddata")]
        public CheckOutAssetsDTO getloaddata([FromBody] CheckOutAssetsDTO data)
        {
            return _AT.getloaddata(data);
        }

        [Route("getitems")]
        public CheckOutAssetsDTO getitems([FromBody] CheckOutAssetsDTO data)
        {
            return _AT.getitems(data);
        }
        [Route("savedetails")]
        public CheckOutAssetsDTO savedetails([FromBody] CheckOutAssetsDTO data)
        {
            return _AT.savedetails(data);
        }
        [Route("deactive")]
        public CheckOutAssetsDTO deactive([FromBody] CheckOutAssetsDTO data)
        {
            return _AT.deactive(data);
        }
        [Route("getcontactperson")]
        public CheckOutAssetsDTO getcontactperson([FromBody] CheckOutAssetsDTO data)
        {
            return _AT.getcontactperson(data);
        }
        [Route("get_avaiablestock")]
        public CheckOutAssetsDTO get_avaiablestock([FromBody] CheckOutAssetsDTO data)
        {
            return _AT.get_avaiablestock(data);
        }




    }
}
