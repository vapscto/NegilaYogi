using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    public class ECSBulkImportController : Controller
    {
        ECSBulkImportDelegate ecsbulkimport = new ECSBulkImportDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet]
        [Route("getalldetails123/{id:int}")]
        public ECSBulkImportDTO getalldetails123(int id)
        {
            ECSBulkImportDTO data = new ECSBulkImportDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return ecsbulkimport.getdata123(data);
        }

        [Route("checkvalidation")]
        public ECSBulkImportDTO checkvalidation([FromBody] ECSBulkImportDTO data)
        {
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ecsbulkimport.checkvalidation(data);
        }

    }
}
