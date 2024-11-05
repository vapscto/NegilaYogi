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
    public class FeeITReceiptReportFacadeController : Controller
    {
        public FeeITReceiptReportInterface _feetar;

        public FeeITReceiptReportFacadeController(FeeITReceiptReportInterface maspag)
        {
            _feetar = maspag;
        }

        // GET: api/values
       
        [HttpPost]
        [Route("getalldetails123")]
        public FeeITReceiptDTO Getdet([FromBody] FeeITReceiptDTO data)
        {
            return _feetar.getdata123(data);
        }
        [Route("getgroupmappedheads")]
        public FeeITReceiptDTO getstuddetails([FromBody]FeeITReceiptDTO value)
        {
            return _feetar.getstuddet(value);
        }
        [Route("getreport")]
        public Task<FeeITReceiptDTO> getreport([FromBody] FeeITReceiptDTO data)
        {
            return _feetar.getreport(data);
        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        //selectacademicyear
        [Route("selectacademicyear")]
        public FeeITReceiptDTO selectacademicyear([FromBody] FeeITReceiptDTO data)
        {
            return _feetar.selectacademicyear(data);
        }
    }
}
