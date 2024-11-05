using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Fees.Tally;
using IVRMUX.Delegates.com.vapstech.Fees.Tally;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Fees.Tally
{
    [Route("api/[controller]")]
    [ValidateAntiForgeryToken]
    public class FeeTallyTransactionController : Controller
    {
        FeeTallyTransactionDelegate od = new FeeTallyTransactionDelegate();

        [Route("getalldetails")]
        public TallyMTransactionDTO load(TallyMTransactionDTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            pgmodu.TMT_VoucherNo= Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.getdata(pgmodu);
        } 

        [Route("SHOWSTUDENT")]
        public TallyMTransactionDTO getheaddetails([FromBody] TallyMTransactionDTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            // pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            pgmodu.TMT_VoucherNo = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.getstudentdetails(pgmodu);
        }

        [Route("savedata")]
        public TallyMTransactionDTO savedata([FromBody] TallyMTransactionDTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //   pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            pgmodu.TMT_VoucherNo = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.savedata(pgmodu);
        }
        [Route("deletedata")]
        public TallyMTransactionDTO deletedata([FromBody] TallyMTransactionDTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //  pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            pgmodu.TMT_VoucherNo = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.deletedata(pgmodu);
        }

        [Route("Concessiondata")]
        public TallyMTransactionDTO Concessiondata([FromBody] TallyMTransactionDTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //  pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            pgmodu.TMT_VoucherNo = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.Concessiondata(pgmodu);
        }


        [Route("Paymentdata")]
        public TallyMTransactionDTO Paymentdata([FromBody] TallyMTransactionDTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //  pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            pgmodu.TMT_VoucherNo = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return od.Paymentdata(pgmodu);
        }



        //[HttpGet]
        //[Route("getalldetails/{id:int}")]
        //public TallyMTransactionDTO getalldetails(int id)
        //{
        //    TallyMTransactionDTO mi_id = new TallyMTransactionDTO();
        //    long MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return od.getalldetails(MI_Id);
        //}
        
        
        [Route("gettallydetails")]
        public TallyMTransactionDTO gettallydetails([FromBody] TallyMTransactionDTO data)
        {
            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.TMT_VoucherNo = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.getalldetails(data);
        }

        [Route("getvouchertypedetails")]
        public TallyMTransactionDTO getvouchertypedetails([FromBody] TallyMTransactionDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.TMT_VoucherNo = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.getvouchertypedetails(data);
        }
        //=============tally reports==================

         [Route("get_tally_data")]
        public TallyMTransactionDTO get_tally_data([FromBody] TallyMTransactionDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.TMT_VoucherNo = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.get_tally_data(data);
        }

    }
}
