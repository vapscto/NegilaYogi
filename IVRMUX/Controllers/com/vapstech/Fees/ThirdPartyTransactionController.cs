using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fee;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    public class ThirdPartyTransactionController : Controller
    {
        ThirdPartyTransactionDelegate objdel = new ThirdPartyTransactionDelegate();


        // GET: api/values getgrpdetails
        [HttpGet]     
        [Route("getdetails/{id:int}")]
        public ThirdPartyTransactionDTO getdetails(int id)
        {
            ThirdPartyTransactionDTO data = new ThirdPartyTransactionDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return objdel.getdetails(data);
        }
        [HttpPost]
        [Route("printreceipt")]
        public ThirdPartyTransactionDTO printreceipt([FromBody]ThirdPartyTransactionDTO data)
        {
            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
           // data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return objdel.printreceipt(data);
        }


       
        [Route("getgrpdetails")]
        public ThirdPartyTransactionDTO getgrpdetails([FromBody]ThirdPartyTransactionDTO data)
        {    
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

           // data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
         
            return objdel.getgrpdetails(data);
        }

            
          //  [HttpPost]
        [Route("getStudtdetails")]
        public ThirdPartyTransactionDTO getStudtdetails([FromBody]ThirdPartyTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
         
            return objdel.getStudtdetails(data);
        }
        [Route("SaveStudentgroupdata")]
        public ThirdPartyTransactionDTO SaveStudentgroupdata([FromBody] ThirdPartyTransactionDTO data)
        {
            data.user_id= Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdel.SaveStudentgroupdata(data);
        }
        [Route("Ckeck_Receipt")]
        public ThirdPartyTransactionDTO Ckeck_Receipt([FromBody] ThirdPartyTransactionDTO data)
        {
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdel.Ckeck_Receipt(data);
        }

        [Route("editOthtransaction")]
        public ThirdPartyTransactionDTO editOthtransaction([FromBody] ThirdPartyTransactionDTO data)
        {
           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.editOthtransaction(data);
        }
        //DeletOthrRecord
        [Route("DeletOthrRecord")]
        public ThirdPartyTransactionDTO DeletOthrRecord([FromBody] ThirdPartyTransactionDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdel.DeletOthrRecord(data);
        }

        
    }
}
