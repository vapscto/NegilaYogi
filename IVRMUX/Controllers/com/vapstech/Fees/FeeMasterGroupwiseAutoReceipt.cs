using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Fees;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeeMasterGroupwiseAutoReceipt : Controller
    {
        FeeMasterGroupwiseAutoReceiptDelegate del = new FeeMasterGroupwiseAutoReceiptDelegate();
        [Route("getdetails")]
        public Fee_Groupwise_AutoReceiptDTO getdetails([FromBody] Fee_Groupwise_AutoReceiptDTO data)
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
        public Fee_Groupwise_AutoReceiptDTO savedetails([FromBody] Fee_Groupwise_AutoReceiptDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return del.savedata(data);
        }

        [Route("edit")]
        public Fee_Groupwise_AutoReceiptDTO EDIT([FromBody] Fee_Groupwise_AutoReceiptDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

          

            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return del.editdata(data);
        }

        [Route("delete")]
        public Fee_Groupwise_AutoReceiptDTO deletee([FromBody] Fee_Groupwise_AutoReceiptDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return del.deletedata(data);
        }

        [Route("getacademicyear")]
        public Fee_Groupwise_AutoReceiptDTO getacademicyear([FromBody] Fee_Groupwise_AutoReceiptDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return del.getacademicyear(data);
        }

        [Route("printreceipt")]
        public Fee_Groupwise_AutoReceiptDTO printrec([FromBody] Fee_Groupwise_AutoReceiptDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;


            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return del.printrecdelegate(data);
        }
        [Route("get_groupdetails")]
        public Fee_Groupwise_AutoReceiptDTO get_groupdetails([FromBody] Fee_Groupwise_AutoReceiptDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return del.get_groupdetails(data);

        }
    }
}


