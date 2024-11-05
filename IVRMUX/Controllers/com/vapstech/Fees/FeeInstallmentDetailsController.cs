using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.AspNetCore.Http;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeeInstallmentDetailsController : Controller

    {
        private static FacadeUrl _config;
        FeeInstallmentDetailsDelegate feeinst = new FeeInstallmentDetailsDelegate();
        private FacadeUrl fdu = new FacadeUrl();
       

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeInstallmentsDetailsDTO Get(int id)
        {
            FeeInstallmentsDetailsDTO data = new FeeInstallmentsDetailsDTO();
            long mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;

            long UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            long ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;
            return feeinst.getdata(data);

        }

    }
}
