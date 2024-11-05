using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
//using DomainModel.Model.com.vapstech.p;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using IVRMUX.Delegates.com.vapstech.Portals.Chairman;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class NewChairmanDashboardController : Controller
    {


        NewChairmanDashboardDelegate crStr = new NewChairmanDashboardDelegate();


        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5

        [HttpGet]
        [Route("Getdetails")]
        public NewChairmanDashboardDTO Getdetails(NewChairmanDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
           // data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
           // data.PaymentNootificationChairman = Convert.ToInt64(HttpContext.Session.GetInt32("PaymentNootificationChairman"));
            return crStr.Getdetails(data);
        }





        [Route("ViewFiles")]
        public NewChairmanDashboardDTO ViewFiles([FromBody] NewChairmanDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return crStr.ViewFiles(data);
        }



    }

}

