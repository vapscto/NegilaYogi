using FeeServiceHub.com.vaps.interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeReceiptImportFacade : Controller
    {
        public FeeReceiptImportInterface _objInter;
        public FeeReceiptImportFacade(FeeReceiptImportInterface data)
        {
            _objInter = data;
        }

        [Route("Savedata")]
        public FeeReceiptImportDTO Savedata([FromBody] FeeReceiptImportDTO data)
        {
            return _objInter.Savedata(data);
        }
        [Route("getdetails/{id:int}")]
        public FeeReceiptImportDTO getdetails(int id)
        {
            return _objInter.getdetails(id);
        }

        [Route("deactiveY")]
        public FeeReceiptImportDTO deactiveY([FromBody] FeeReceiptImportDTO data)
        {
            return _objInter.deactiveY(data);
        }
    }
}
