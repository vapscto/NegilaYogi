using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeheadFacade : Controller
    {
        //// GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        public FeeHeadInterface _feegroupHeadpage;

        public FeeheadFacade(FeeHeadInterface maspag)
        {
            _feegroupHeadpage = maspag;
        }

        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public FeeHeadDTO getpagedetails(int id)
        {
            // id = 12;
            return _feegroupHeadpage.getpageedit(id);
        }

        // GET: api/values
        [HttpGet]
        public FeeHeadDTO Get(FeeHeadDTO mas)
        {
            return _feegroupHeadpage.GetGroupSearchData(mas);
        }


        [Route("changeorderData")]
        public FeeHeadDTO changeorderData([FromBody] FeeHeadDTO mas)
        {
            return _feegroupHeadpage.changeorderData(mas);
        }

        // GET api/values/5
        [Route("getdetails/{id:int}")]
        public FeeHeadDTO getorgdet(int id)
        {
            return _feegroupHeadpage.getdetails(id);
        }

        // POST api/values
        [HttpPost]
        public FeeHeadDTO Post([FromBody] FeeHeadDTO org)
        {
            return _feegroupHeadpage.SaveGroupData(org);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public string Put(int id, [FromBody] FeeHeadDTO value)
        {
            return "success";
        }


        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public FeeHeadDTO Deleterec(int id)
        {
            return _feegroupHeadpage.deleterec(id);
        }


        [HttpPost]
        [Route("deactivate")]
        public FeeHeadDTO deactivateAcdmYear([FromBody] FeeHeadDTO id)
        {
            // id = 12;
            return _feegroupHeadpage.deactivate(id);
        }
    }
}
