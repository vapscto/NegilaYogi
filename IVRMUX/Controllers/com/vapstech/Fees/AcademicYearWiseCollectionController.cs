using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Fees;
using corewebapi18072016.Delegates.com.vapstech.Fees;
using IVRMUX.Delegates.com.vapstech.Fees;

namespace IVRMUX.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    public class AcademicYearWiseCollectionController : Controller
    {
        AcademicYearWiseCollectionDelegate FCWR = new AcademicYearWiseCollectionDelegate();



        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public CategoryWiseFeeCollectionDTO Get(int id)
        {
            CategoryWiseFeeCollectionDTO data = new CategoryWiseFeeCollectionDTO();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return FCWR.getdetails(data);
        }



        [HttpPost]
        [Route("radiobtndata")]
        public CategoryWiseFeeCollectionDTO radiobtndata([FromBody]CategoryWiseFeeCollectionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            data.amstid = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return FCWR.radiobtndata(data);
        }



        [HttpPost]
        [Route("onchangeacademic")]
        public CategoryWiseFeeCollectionDTO onchangeacademic([FromBody]CategoryWiseFeeCollectionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return FCWR.onchangeacademic(data);
        }
    }
}
