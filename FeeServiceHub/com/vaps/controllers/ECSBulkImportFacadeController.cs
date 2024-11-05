using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;
using FeeServiceHub.com.vaps.interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class ECSBulkImportFacadeController : Controller
    {

        public ECSBulkImportInterface _ecsimport;

        public ECSBulkImportFacadeController(ECSBulkImportInterface maspag)
        {
            _ecsimport = maspag;
        }


        [HttpPost]
        [Route("getalldetails123")]
        public ECSBulkImportDTO Getdet([FromBody] ECSBulkImportDTO data)
        {
            return _ecsimport.getdata123(data);
        }

        [HttpPost]
        [Route("checkvalidation")]
        public Task<ECSBulkImportDTO> checkvalidation([FromBody] ECSBulkImportDTO data)
        {
            return _ecsimport.checkvalidation(data);
        }
    }
}
