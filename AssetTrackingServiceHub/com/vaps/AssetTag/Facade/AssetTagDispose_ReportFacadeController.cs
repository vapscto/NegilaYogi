using AssetTrackingServiceHub.com.vaps.AssetTag.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Controllers
{
    [Route("api/[controller]")]
    public class AssetTagDispose_ReportFacadeController : Controller
    {
        // GET: api/values
        AssetTagDispose_ReportInterface _Inv;
        public AssetTagDispose_ReportFacadeController(AssetTagDispose_ReportInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public Task<AssetTagDisposeDTO> getloaddata([FromBody] AssetTagDisposeDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [Route("onreport")]
        public Task<AssetTagDisposeDTO> onreport([FromBody] AssetTagDisposeDTO data)
        {
            return _Inv.onreport(data);
        }

        


    }
}
