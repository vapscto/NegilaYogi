using IVRMUX.Delegates.com.vapstech.AssetTracking;
using IVRMUX.Delegates.com.vapstech.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.AssetTracking
{
    [Route("api/[controller]")]
    public class SiteLocationsReportController : Controller
    {
        SiteLocationsReportDelegate _delegate = new SiteLocationsReportDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public AT_MasterSiteDTO getloaddata(int id)
        {
            AT_MasterSiteDTO data = new AT_MasterSiteDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }
       
        [Route("getreport")]
        public AT_MasterSiteDTO getreport([FromBody] AT_MasterSiteDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getreport(data);
        }
         [HttpGet]
        [Route("get_all_data_LCR/{id:int}")]
        public AT_MasterSiteDTO showdetails_LCR(int id)
        {
            AT_MasterSiteDTO data = new AT_MasterSiteDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.get_all_data_LCR(data);
        }
       
        [Route("getreport_LCR")]
        public AT_MasterSiteDTO showdetails_LCR([FromBody] AT_MasterSiteDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getreport_LCR(data);
        }


    }
}
