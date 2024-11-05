using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.admission;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeReceiptReportFacadeController : Controller
    {
        public FeeReceiptReportInterface _feetar;

        public FeeReceiptReportFacadeController(FeeReceiptReportInterface maspag)
        {
            _feetar = maspag;
        }

     
      
        [HttpPost]
        [Route("getalldetails123")]
        public FeeReceiptDTO Getdet([FromBody] FeeReceiptDTO data)
        {
            return _feetar.getdata123(data);
        }
        [Route("getinsdetils")]
        public FeeReceiptDTO getinsdetils([FromBody] FeeReceiptDTO data)
        {
            return _feetar.getinsdetils(data);
        }

        [Route("getreport")]
        public Task<FeeReceiptDTO> getreport([FromBody] FeeReceiptDTO data)
        {
            return _feetar.getreport(data);
        }
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
