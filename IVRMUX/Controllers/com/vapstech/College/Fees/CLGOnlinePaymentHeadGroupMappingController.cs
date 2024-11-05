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
    public class CLGOnlinePaymentHeadGroupMappingController : Controller
    {
       
        // GET: api/values
        public CLGOnlinePaymentHeadGroupMappingDelegate objDel = new CLGOnlinePaymentHeadGroupMappingDelegate();

      
        [Route("onlineMappingDetails/{id:int}")]
        public Clg_StudentFeeGroupMapping_DTO onlineMappingDetails(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.onlineMappingDetails(id);
        }
       

        [Route("edit/{id:int}")]
        public Clg_StudentFeeGroupMapping_DTO edit(int id)
        {
            return objDel.edit(id);
        }

        [Route("delete/{id:int}")]
        public Clg_StudentFeeGroupMapping_DTO delete(int id)
        {
            return objDel.delete(id);
        }

        [HttpPost]
     
        public Clg_StudentFeeGroupMapping_DTO save([FromBody]Clg_StudentFeeGroupMapping_DTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.save(id);
        }

        //[HttpPost]
        [Route("groupsel")]
        public Clg_StudentFeeGroupMapping_DTO groupsel([FromBody]Clg_StudentFeeGroupMapping_DTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.groupsel(id);
        }
        //[HttpPost]
        [Route("headsel")]
        public Clg_StudentFeeGroupMapping_DTO headsel([FromBody]Clg_StudentFeeGroupMapping_DTO Data)
        {
            Data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.headsel(Data);
        }
    

        [Route("academicsel")]
        public Clg_StudentFeeGroupMapping_DTO academicsel([FromBody] Clg_StudentFeeGroupMapping_DTO pgmodu)
        {
            
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objDel.academicsel(pgmodu);
        }

     
    }
}
