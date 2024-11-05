using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeReceiptImportStthomasFacade : Controller
    {
        public FeeReceiptImportStthomasInterface _objInter;
        public FeeReceiptImportStthomasFacade(FeeReceiptImportStthomasInterface data)
        {
            _objInter = data;
        }

        //[Route("Savedata")]
        //public FeeReceiptImportStthomasDTO Savedata([FromBody] FeeReceiptImportStthomasDTO data)
        //{
        //    return _objInter.Savedata(data);
        //}


        [Route("Savedata")]
        public async Task<FeeReceiptImportStthomasDTO> Savedata([FromBody] FeeReceiptImportStthomasDTO data)
        {

            return await _objInter.Savedata(data);
        }

        [Route("getdetails/{id:int}")]
        public FeeReceiptImportStthomasDTO getdetails(int id)
        {
            return _objInter.getdetails(id);
        }

        [Route("deactiveY")]
        public FeeReceiptImportStthomasDTO deactiveY([FromBody] FeeReceiptImportStthomasDTO data)
        {
            return _objInter.deactiveY(data);
        }
        //Added By PraveenGouda
        [Route("getreportdetails")]
        public FeeReceiptImportStthomasDTO getreportdetails([FromBody] FeeReceiptImportStthomasDTO data)
        {
            return _objInter.getreportdetails(data);
        }
        [Route("deletereceipt")]
        public FeeReceiptImportStthomasDTO deletereceipt([FromBody] FeeReceiptImportStthomasDTO data)
        {
            return _objInter.deletereceipt(data);
        }

    }
}
