using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fee;
using corewebapi18072016.Delegates.com.vapstech.College.Fees.Masters;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Fees.Masters
{
    [Route("api/[controller]")]
    public class ClgDatewiseHeadCollectionController : Controller
    {
       
        // GET: api/values
        public ClgDatewiseHeadCollectionDelegate objDel = new ClgDatewiseHeadCollectionDelegate();

        [HttpGet]
        [Route("GetYearList/{id:int}")]
        public ClgDatewiseHeadCollectionDTO GetYearList(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.GetYearList(id);
        }
        [HttpPost]
        [Route("get_feegroups")]
        public ClgDatewiseHeadCollectionDTO get_feegroups([FromBody]ClgDatewiseHeadCollectionDTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            id.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objDel.get_feegroups(id);
        }

        //[HttpPost]
        [Route("get_heads")]
        public ClgDatewiseHeadCollectionDTO get_heads([FromBody]ClgDatewiseHeadCollectionDTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.get_heads(id);
        }
        //[HttpPost]
        [Route("get_semisters")]
        public ClgDatewiseHeadCollectionDTO get_semisters([FromBody]ClgDatewiseHeadCollectionDTO Data)
        {
            Data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.get_semisters(Data);
        }
      //[HttpPost]
        [Route("get_report")]
        public ClgDatewiseHeadCollectionDTO get_report([FromBody]ClgDatewiseHeadCollectionDTO Data)
        {
            Data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            Data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objDel.get_report(Data);
        }

        [Route("savedata")]
        public ClgDatewiseHeadCollectionDTO savedata([FromBody] ClgDatewiseHeadCollectionDTO pgmodu)
        {
            
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //pgmodu.MI_Id = 2;
            //pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objDel.savedata(pgmodu);
        }

      [Route("editdata")]
      public ClgDatewiseHeadCollectionDTO editdata([FromBody]ClgDatewiseHeadCollectionDTO data)
        {
            data.MI_Id =Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id= Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return objDel.editdata(data);
        }

        [Route("DeleteRecord")]
        public ClgDatewiseHeadCollectionDTO DeleteRecord([FromBody] ClgDatewiseHeadCollectionDTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           // id.FCMSGH_Id = Convert.ToInt32(HttpContext.Session.GetInt32("FCMSGH_Id"));
            return objDel.DeleteRecord(id);
        }
    }
}
