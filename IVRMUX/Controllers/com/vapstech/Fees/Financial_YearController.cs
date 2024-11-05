using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Fees.Tally;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    public class Financial_YearController : Controller
    {

        Financial_YearDelegate del = new Financial_YearDelegate();
        [HttpPost]
        [Route("loaddata")]
        public Financial_YearDTO loaddata([FromBody] Financial_YearDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loaddata(data);

        }
        [Route("save")]
        public Financial_YearDTO save([FromBody]Financial_YearDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.save(data);
        }
        

    }
}
