using AssetTrackingServiceHub.com.vaps.AssetTag.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Controllers
{
    [Route("api/[controller]")]
    public class AssetTagCheckout_ReportFacadeController : Controller
    {
        // GET: api/values
        AssetTagCheckout_ReportInterface _Inv;
        public AssetTagCheckout_ReportFacadeController(AssetTagCheckout_ReportInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public Task<AssetTagCheckOutDTO> getloaddata([FromBody] AssetTagCheckOutDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [Route("onreport")]
        public Task<AssetTagCheckOutDTO> onreport([FromBody] AssetTagCheckOutDTO data)
        {
            return _Inv.onreport(data);
        }

        


    }
}
