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
    public class DisposeReportFacadeController : Controller
    {
        // GET: api/values
        DisposeReportInterface _AT;
        public DisposeReportFacadeController(DisposeReportInterface AT)
        {
            _AT = AT;
        }
        [Route("getloaddata")]
        public DisposeAssetsDTO getloaddata([FromBody] DisposeAssetsDTO data)
        {
            return _AT.getloaddata(data);
        }

       
        [Route("getreport")]
        public Task<DisposeAssetsDTO> getreport([FromBody] DisposeAssetsDTO data)
        {
            return _AT.getreport(data);
        }
      




    }
}
