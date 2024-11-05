using AssetTrackingServiceHub.com.vaps.AssetTag.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Controllers
{
    [Route("api/[controller]")]
    public class AssetTagCheckIn_ReportFacadeController : Controller
    {
        // GET: api/values
        AssetTagCheckIn_ReportInterface _Inv;
        public AssetTagCheckIn_ReportFacadeController(AssetTagCheckIn_ReportInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public Task<AssetTagCheckInDTO> getloaddata([FromBody] AssetTagCheckInDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [Route("onreport")]
        public Task<AssetTagCheckInDTO> onreport([FromBody] AssetTagCheckInDTO data)
        {
            return _Inv.onreport(data);
        }

        


    }
}
