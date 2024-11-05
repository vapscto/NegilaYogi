using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VidyaBharathi;
using VidyaBharathiServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace VidyaBharathiServiceHub.com.vaps.Controllers
{

    [Route("api/[controller]")]
    public class VBSC_Master_SportsCCName_UOMFacade : Controller
    {
        VBSC_Master_SportsCCName_UOMInterface _interface;
        public VBSC_Master_SportsCCName_UOMFacade(VBSC_Master_SportsCCName_UOMInterface interfaces)
        {
            _interface = interfaces;
        }

        [Route("getDetails")]
        public VBSC_Master_SportsCCName_UOMDTO getDetails([FromBody]VBSC_Master_SportsCCName_UOMDTO data)
        {
            return _interface.getDetails(data);
        }
        

        [Route("save")]
        public VBSC_Master_SportsCCName_UOMDTO save([FromBody]VBSC_Master_SportsCCName_UOMDTO data)
        {
            return _interface.saveRecord(data);
        }

        [Route("EditDetails/{id:int}")]
        public VBSC_Master_SportsCCName_UOMDTO EditDetails(int id)
        {
            return _interface.EditDetails(id);
        }

        [Route("deactivate")]
        public VBSC_Master_SportsCCName_UOMDTO deactivate([FromBody]VBSC_Master_SportsCCName_UOMDTO data)
        {
            return _interface.deactivate(data);
        }
    }
}

