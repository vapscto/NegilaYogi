using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class TransactionNumberingController : Controller
    {
        TransactionNumberingDelegate enqu = new TransactionNumberingDelegate();

        [HttpPost]
        public Master_NumberingDTO saveMaster_Numberingdetail([FromBody] Master_NumberingDTO en)
        {
            en.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            en.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return enqu.saveMaster_Numberingdetails(en);
        }

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public Master_NumberingDTO getdetail(int id)
        {
            MandatoryFieldsDTO mand = new MandatoryFieldsDTO();
            int moid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MO_Id"));
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            mand.MI_Id = mid;
            mand.MO_Id = moid;
            mand.IVRMP_Id = id;
            return enqu.Master_NumberingDetails(mand);
        }

        [Route("deleteRollnoconfig/{id:int}")]
        public Master_NumberingDTO deleteRollnoconfig(int id)
        {
            Master_NumberingDTO mand = new Master_NumberingDTO();
            int moid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MO_Id"));
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            mand.MI_Id = mid;
            mand.IVRMARNC_Id = id;

            return enqu.deleteRollnoconfig(mand);
        }
    }
}
