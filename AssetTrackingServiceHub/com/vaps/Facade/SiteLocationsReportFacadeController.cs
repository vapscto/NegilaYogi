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
    public class SiteLocationsReportFacadeController : Controller
    {
        // GET: api/values
        SiteLocationsReportInterface _AT;
        public SiteLocationsReportFacadeController(SiteLocationsReportInterface AT)
        {
            _AT = AT;
        }
        [Route("getloaddata")]
        public AT_MasterSiteDTO getloaddata([FromBody] AT_MasterSiteDTO data)
        {
            return _AT.getloaddata(data);
        }

       
        [Route("getreport")]
        public AT_MasterSiteDTO getreport([FromBody] AT_MasterSiteDTO data)
        {
            return _AT.getreport(data);
        }
        [Route("get_all_data_LCR")]
        public AT_MasterSiteDTO get_all_data_LCR([FromBody] AT_MasterSiteDTO data)
        {
            return _AT.get_all_data_LCR(data);
        }

       
        [Route("getreport_LCR")]
        public AT_MasterSiteDTO getreport_LCR([FromBody] AT_MasterSiteDTO data)
        {
            return _AT.getreport_LCR(data);
        }
      




    }
}
