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
    public class CheckInReportFacadeController : Controller
    {
        // GET: api/values
        CheckInReportInterface _AT;
        public CheckInReportFacadeController(CheckInReportInterface AT)
        {
            _AT = AT;
        }
        [Route("getloaddata")]
        public CheckInAssetsDTO getloaddata([FromBody] CheckInAssetsDTO data)
        {
            return _AT.getloaddata(data);
        }

       
        [Route("getreport")]
        public Task<CheckInAssetsDTO> getreport([FromBody] CheckInAssetsDTO data)
        {
            return _AT.getreport(data);
        }
      




    }
}
