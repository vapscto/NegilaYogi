using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

namespace IVRMUX.Controllers.com.vapstech.Library
{
    [Route("api/[controller]")]
    public class PurchaseDonateReportController : Controller
    {

        PurchaseDonateReportDelegate _delobj = new PurchaseDonateReportDelegate();
      
    
        [Route("getdata")]
        public CirculationParameterDTO getdata([FromBody] CirculationParameterDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getdata(data);
        }


    }
}
