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
    public class FeeOpeningBalanceFacadeController : Controller
    {
        public FeeOpeningBalanceInterface _feetar;

        public FeeOpeningBalanceFacadeController(FeeOpeningBalanceInterface maspag)
        {
            _feetar = maspag;
        }

        // GET: api/values
   
        [HttpPost]
        [Route("getalldetails123")]
        public FeeOpeningBalanceDTO Getdet([FromBody] FeeOpeningBalanceDTO data)
        {
            return _feetar.getdata123(data);
        }

        [HttpPost]
        [Route("onselectacademicyear")]
        public FeeOpeningBalanceDTO onselectacademicyear([FromBody] FeeOpeningBalanceDTO data)
        {
            return _feetar.onselectacademicyear(data);
        }

        [Route("getgroupmappedheads")]
        public FeeOpeningBalanceDTO getstuddetails([FromBody] FeeOpeningBalanceDTO value)
        {
            return _feetar.getstuddet(value);
        }
        [Route("getrefund")]
        public FeeOpeningBalanceDTO getrefund([FromBody] FeeOpeningBalanceDTO value)
        {
            return _feetar.getrefund(value);
        }
        [Route("getreport")]
        public FeeOpeningBalanceDTO getreport([FromBody] FeeOpeningBalanceDTO data)
        {
            return _feetar.getreport(data);
        }

        [Route("getclshead")]
        public FeeOpeningBalanceDTO getclshead([FromBody] FeeOpeningBalanceDTO data)
        {
            return _feetar.getclshead(data);
        }
        [Route("getgroup")]
        public FeeOpeningBalanceDTO getgroup([FromBody] FeeOpeningBalanceDTO data)
        {
            return _feetar.getgroup(data);
        }
        [Route("gethead")]
        public FeeOpeningBalanceDTO gethead([FromBody] FeeOpeningBalanceDTO data)
        {
            return _feetar.gethead(data);
        }

        [Route("filterstudents")]
        public FeeOpeningBalanceDTO filterstudents([FromBody] FeeOpeningBalanceDTO value)
        {
            return _feetar.filterstudents(value);
        }

        [HttpPost]
        [Route("savedata")]
        public FeeOpeningBalanceDTO getclassstudentlist([FromBody]FeeOpeningBalanceDTO student)
        {
            return _feetar.getlisttwo(student);
        }

        [HttpPost]
        [Route("DeleteEntry")]
        public FeeOpeningBalanceDTO DeleteEntry([FromBody]FeeOpeningBalanceDTO data)
        {
            return _feetar.DeleteEntry(data);
        }
        [HttpPost]
        [Route("searching")]
        public FeeOpeningBalanceDTO searching([FromBody] FeeOpeningBalanceDTO data)
        {
            return _feetar.searching(data);
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
