using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Library.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Library.Reports
{
    [Route("api/[controller]")]
    public class OpacSearchController : Controller
    {
        OpacSearchDelegate del = new OpacSearchDelegate();


        [Route("getalldetails")]
        public OpacSearchDTO getalldetails([FromBody] OpacSearchDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getalldetails(data);
        }
        [Route("report")]
        public OpacSearchDTO report([FromBody]OpacSearchDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.report(data);
        }
    }
}
