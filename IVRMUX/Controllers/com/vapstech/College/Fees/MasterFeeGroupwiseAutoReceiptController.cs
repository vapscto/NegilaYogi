using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.College.Fees;
using PreadmissionDTOs.com.vaps.College.Fees;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Fees
{
    [Route("api/[controller]")]
    public class MasterFeeGroupwiseAutoReceiptController : Controller
    {
        MasterFeeGroupwiseAutoReceiptDelegate del = new MasterFeeGroupwiseAutoReceiptDelegate();

        [HttpPost]
        [Route("getdetails")]
        public MasterFeeGroupwiseAutoReceiptDTO getdetails([FromBody] MasterFeeGroupwiseAutoReceiptDTO data)

        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            string acadyear = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return del.getdata(data);
        }

        [Route("savedata")]
        public MasterFeeGroupwiseAutoReceiptDTO savedetails([FromBody] MasterFeeGroupwiseAutoReceiptDTO data)
        {
            //int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.MI_Id = mid;
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            


            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return del.savedata(data);
        }

        [Route("edit")]
        public MasterFeeGroupwiseAutoReceiptDTO EDIT([FromBody] MasterFeeGroupwiseAutoReceiptDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;



            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return del.editdata(data);
        }

        [Route("delete")]
        public MasterFeeGroupwiseAutoReceiptDTO deletee([FromBody] MasterFeeGroupwiseAutoReceiptDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return del.deletedata(data);
        }
    }
}
