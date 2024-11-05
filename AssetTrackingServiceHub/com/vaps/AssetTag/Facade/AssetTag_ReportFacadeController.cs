using AssetTrackingServiceHub.com.vaps.AssetTag.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Controllers
{
    [Route("api/[controller]")]
    public class AssetTag_ReportFacadeController : Controller
    {
        // GET: api/values
        AssetTag_ReportInterface _Inv;
        public AssetTag_ReportFacadeController(AssetTag_ReportInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public Task<AssetTagDTO> getloaddata([FromBody] AssetTagDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [Route("onreport")]
        public Task<AssetTagDTO> onreport([FromBody] AssetTagDTO data)
        {
            return _Inv.onreport(data);
        }

        


    }
}
