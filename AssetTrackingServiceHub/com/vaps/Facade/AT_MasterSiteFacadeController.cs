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
    public class AT_MasterSiteFacadeController : Controller
    {
        // GET: api/values
        AT_MasterSiteInterface _AT;
        public AT_MasterSiteFacadeController(AT_MasterSiteInterface AT)
        {
            _AT = AT;
        }
        [Route("getloaddata")]
        public AT_MasterSiteDTO getloaddata([FromBody] AT_MasterSiteDTO data)
        {
            return _AT.getloaddata(data);
        }
        [Route("savedetails")]
        public AT_MasterSiteDTO savedetails([FromBody] AT_MasterSiteDTO data)
        {
            return _AT.savedetails(data);
        }
        [Route("deactive")]
        public AT_MasterSiteDTO deactive([FromBody] AT_MasterSiteDTO data)
        {
            return _AT.deactive(data);
        }




    }
}
