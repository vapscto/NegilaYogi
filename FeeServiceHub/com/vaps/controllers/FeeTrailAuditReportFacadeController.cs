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
    public class FeeTrailAuditReportFacadeController : Controller
    {
        public FeeTrailAuditReportInterface _feetar;

        public FeeTrailAuditReportFacadeController(FeeTrailAuditReportInterface maspag)
        {
            _feetar = maspag;
        }

        // GET: api/values
        [HttpGet]
        [Route("getdetails/{id:int}")]
        public FeeTrailAuditDTO Get(int id)
        {
            return _feetar.getdetails(id);
        }
        [HttpPost]
        [Route("getalldetails123")]
        public FeeTrailAuditDTO Getdet([FromBody] FeeTrailAuditDTO data)
        {
            return _feetar.getdata123(data);
        }
        //[Route("getreport")]
        //public FeeTrailAuditDTO getreport([FromBody] FeeTrailAuditDTO data)
        //{
        //    return _feetar.getreport(data);
        //}
        [Route("getreport")]
        public FeeTrailAuditDTO getreport([FromBody] FeeTrailAuditDTO data)
        {
            return _feetar.getreport(data);
        }
        [Route("viewdetails")]
        public FeeTrailAuditDTO viewdetails([FromBody] FeeTrailAuditDTO data)
        {
            return _feetar.viewdetails(data);
        }
        

       [Route("searchfilter")]
        public FeeTrailAuditDTO searchfilter([FromBody] FeeTrailAuditDTO data)
        {
            return _feetar.searchfilter(data);
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
    }
}
