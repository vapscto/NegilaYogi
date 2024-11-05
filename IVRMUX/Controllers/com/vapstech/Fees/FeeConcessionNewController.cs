using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    [ValidateAntiForgeryToken]
    public class FeeConcessionNewController : Controller
    {
        FeeConcessionNewDelegate od = new FeeConcessionNewDelegate();
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeConcessionDTO getinitialdropdowns(int id)
        {
            FeeConcessionDTO data = new FeeConcessionDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return od.getdata(data);
        }

        //[Route("onselectclassorcat")]
        //public FeeConcessionDTO selectclassorcat([FromBody] FeeConcessionDTO data)
        //{
        //    int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    data.MI_Id = mid;

        //    //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
        //    data.ASMAY_Id = data.ASMAY_Id;

        //    return od.selectcatorclass(data);
        //}

        //[Route("fillhead")]
        //public FeeConcessionDTO fillheaddetails([FromBody] FeeConcessionDTO data)
        //{
        //    int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    data.MI_Id = mid;

        //    //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
        //    data.ASMAY_Id = data.ASMAY_Id;

        //    return od.fillheaddetailsss(data);
        //}

        [Route("fillamount")]
        public FeeConcessionDTO fillamt([FromBody] FeeConcessionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = data.ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            return od.fillamount(data);
        }

        [Route("savedata")]
        public FeeConcessionDTO savedataa([FromBody] FeeConcessionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = data.ASMAY_Id;

            return od.savedatadelegate(data);
        }


        [Route("Deletedetails")]
        public FeeConcessionDTO deletereceipt([FromBody]FeeConcessionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = data.ASMAY_Id;

            return od.delrec(data);
        }

        [Route("EditconcessionDetails")]
        public FeeConcessionDTO EditconcessionDetails([FromBody]FeeConcessionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.EditconcessionDetails(data);
        }


        //[Route("fillstaff")]
        //public FeeConcessionDTO fillstafff([FromBody]FeeConcessionDTO data)
        //{
        //    int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    data.MI_Id = mid;

        //    int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
        //    data.userid = UserId;

        //    //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
        //    data.ASMAY_Id = data.ASMAY_Id;

        //    return od.fillstaf(data);
        //}


        //[Route("getacademicyear")]
        //public FeeConcessionDTO getacademicye([FromBody]FeeConcessionDTO data)
        //{
        //    int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    data.MI_Id = mid;

        //    int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
        //    data.userid = UserId;

        //    return od.getacadem(data);
        //}

        [Route("getfeegroup")]
        public FeeConcessionDTO getfeegroup([FromBody]FeeConcessionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            return od.getfeegroup(data);
        }

        [Route("getfeehead")]
        public FeeConcessionDTO getfeehead([FromBody]FeeConcessionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            return od.getfeehead(data);
        }


        [Route("getterm")]
        public FeeConcessionDTO getterm([FromBody]FeeConcessionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            return od.getterm(data);
        }
        [Route("getinstallment")]
        public FeeConcessionDTO getinstallment([FromBody]FeeConcessionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            return od.getinstallment(data);
        }
        
             [Route("studentdata1")]
        public FeeConcessionDTO studentdata1([FromBody]FeeConcessionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            return od.studentdata1(data);
        }
        [Route("studentdata")]
        public FeeConcessionDTO studentdata([FromBody]FeeConcessionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            return od.studentdata(data);
        }
        [Route("Save")]
        public FeeConcessionDTO Save([FromBody]FeeConcessionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            return od.Save(data);
        }

    }
}
