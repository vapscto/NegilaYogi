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
    public class FeeYearlyRebateSettingFacade : Controller
    {

        public FeeYearlyRebateSettingInterface _feegroupHeadpage;

        public FeeYearlyRebateSettingFacade(FeeYearlyRebateSettingInterface maspag)
        {
            _feegroupHeadpage = maspag;
        }

        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public FeeYearlyRedateSettingDTO getpagedetails(int id)
        {
            // id = 12;
            return _feegroupHeadpage.getpageedit(id);
        }

        // GET: api/values
        [HttpGet]
        public FeeYearlyRedateSettingDTO Get(FeeYearlyRedateSettingDTO mas)
        {
            return _feegroupHeadpage.GetGroupSearchData(mas);
        }


      

        // GET api/values/5
        [Route("getdetails/{id:int}")]
        public FeeYearlyRedateSettingDTO getorgdet(int id)
        {
            return _feegroupHeadpage.getdetails(id);
        }

        // POST api/values
        [HttpPost]
        public FeeYearlyRedateSettingDTO Post([FromBody] FeeYearlyRedateSettingDTO org)
        {
            return _feegroupHeadpage.SaveGroupData(org);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public string Put(int id, [FromBody] FeeYearlyRedateSettingDTO value)
        {
            return "success";
        }


        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public FeeYearlyRedateSettingDTO Deleterec(int id)
        {
            return _feegroupHeadpage.deleterec(id);
        }


        [HttpPost]
        [Route("deactivate")]
        public FeeYearlyRedateSettingDTO deactivateAcdmYear([FromBody] FeeYearlyRedateSettingDTO id)
        {
            // id = 12;
            return _feegroupHeadpage.deactivate(id);
        }
    }
}
