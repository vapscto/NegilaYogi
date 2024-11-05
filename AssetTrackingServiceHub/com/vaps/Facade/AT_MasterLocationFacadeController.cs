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
    public class AT_MasterLocationFacadeController : Controller
    {
        // GET: api/values
        AT_MasterLocationInterface _AT;
        public AT_MasterLocationFacadeController(AT_MasterLocationInterface AT)
        {
            _AT = AT;
        }
        [Route("getloaddata")]
        public AT_MasterLocationDTO getloaddata([FromBody] AT_MasterLocationDTO data)
        {
            return _AT.getloaddata(data);
        }
        [Route("savedetails")]
        public AT_MasterLocationDTO savedetails([FromBody] AT_MasterLocationDTO data)
        {
            return _AT.savedetails(data);
        }
        [Route("deactive")]
        public AT_MasterLocationDTO deactive([FromBody] AT_MasterLocationDTO data)
        {
            return _AT.deactive(data);
        }
        

        


    }
}
