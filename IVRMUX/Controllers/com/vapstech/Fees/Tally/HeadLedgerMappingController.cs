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
    public class HeadLedgerMappingController : Controller
    {
        HeadLedgerMappingDeleagate od = new HeadLedgerMappingDeleagate();
      
        [Route("getalldetails")]
        public HeadLedgerCodeMapDTO load(HeadLedgerCodeMapDTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.getdata(pgmodu);
        }

        [Route("getheaddetails")]
        public HeadLedgerCodeMapDTO getheaddetails([FromBody] HeadLedgerCodeMapDTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.getheaddetails(pgmodu);
        }

        [Route("getgroupdetails")]
        public HeadLedgerCodeMapDTO getgroupdetails([FromBody] HeadLedgerCodeMapDTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           // pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.getgroupdetails(pgmodu);
        }

        [Route("savedata")]
        public HeadLedgerCodeMapDTO savedata([FromBody] HeadLedgerCodeMapDTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.savedata(pgmodu);
        }
        [Route("deletedata")]
        public HeadLedgerCodeMapDTO deletedata([FromBody] HeadLedgerCodeMapDTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.deletedata(pgmodu);
        }


    }
}












