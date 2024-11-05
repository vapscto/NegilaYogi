using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Fees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    public class FeeReceiptImportStthomasController : Controller
    {

        FeeReceiptImportStthomasDelegate _delobj = new FeeReceiptImportStthomasDelegate();



        public FeeReceiptImportStthomasDTO Savedata([FromBody] FeeReceiptImportStthomasDTO data)
        {
           // data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.Savedata(data);
        }
        [Route("getdetails/{id:int}")]
        public FeeReceiptImportStthomasDTO getdetails(int id)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            id = mi_id;
            return _delobj.getdetails(id);
        }

        [Route("deactiveY")]
        public FeeReceiptImportStthomasDTO deactiveY([FromBody] FeeReceiptImportStthomasDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.deactiveY(data);
        }

        //Added By PraveenGouda       
        [Route("getreportdetails")]
        public FeeReceiptImportStthomasDTO getreportdetails([FromBody]FeeReceiptImportStthomasDTO dto)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.MI_Id = mi_id;
            return _delobj.getreportdetails(dto);
        }

        [Route("Deletereceipt")]
        public FeeReceiptImportStthomasDTO deletereceipt([FromBody]FeeReceiptImportStthomasDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            return _delobj.deletereceipt(data);
        }

    }
}
