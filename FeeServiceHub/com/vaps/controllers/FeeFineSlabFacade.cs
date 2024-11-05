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
    public class FeeFineSlabFacade : Controller
    {
        public FeeFineSlabInterface _feegroupHeadpage;

        public FeeFineSlabFacade(FeeFineSlabInterface maspag)
        {
            _feegroupHeadpage = maspag;
        }

        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public FeeFineSlabDTO getpagedetails(int id)
        {
            // id = 12;
            return _feegroupHeadpage.getpageedit(id);
        }

        // GET: api/values
        [HttpGet]
        public FeeFineSlabDTO Get(FeeFineSlabDTO mas)
        {
            return _feegroupHeadpage.GetGroupSearchData(mas);
        }

        // GET api/values/5
        [Route("getdetails/{id:int}")]
        public FeeFineSlabDTO getorgdet(int id)
        {
            return _feegroupHeadpage.getdetails(id);
        }

        // POST api/values
        [HttpPost]
        public FeeFineSlabDTO Post([FromBody] FeeFineSlabDTO org)
        {
            return _feegroupHeadpage.SaveGroupData(org);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public string Put(int id, [FromBody] FeeFineSlabDTO value)
        {
            return "success";
        }


        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public FeeFineSlabDTO Deleterec(int id)
        {
            return _feegroupHeadpage.deleterec(id);
        }


        [HttpPost]
        [Route("deactivate")]
        public FeeFineSlabDTO deactivateAcdmYear([FromBody] FeeFineSlabDTO id)
        {
            // id = 12;
            return _feegroupHeadpage.deactivate(id);
        }
    }
}
