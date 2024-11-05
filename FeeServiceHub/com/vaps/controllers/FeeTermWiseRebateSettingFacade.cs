using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeTermWiseRebateSettingFacade : Controller
    {

        public FeeTermWiseRebateSettingInterface _feegroupHeadpage;

        public FeeTermWiseRebateSettingFacade(FeeTermWiseRebateSettingInterface maspag)
        {
            _feegroupHeadpage = maspag;
        }

        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public FeeTermWiseRebateSettingDTO getpagedetails(int id)
        {
            // id = 12;
            return _feegroupHeadpage.getpageedit(id);
        }

        // GET: api/values
        [HttpGet]
        public FeeTermWiseRebateSettingDTO Get(FeeTermWiseRebateSettingDTO mas)
        {
            return _feegroupHeadpage.GetGroupSearchData(mas);
        }




        // GET api/values/5
        [Route("getdetails/{id:int}")]
        public FeeTermWiseRebateSettingDTO getorgdet(int id)
        {
            return _feegroupHeadpage.getdetails(id);
        }

        // POST api/values
        [HttpPost]
        public FeeTermWiseRebateSettingDTO Post([FromBody] FeeTermWiseRebateSettingDTO org)
        {
            return _feegroupHeadpage.SaveGroupData(org);
        }

        [HttpPost]
        [Route("deactivate")]
        public FeeTermWiseRebateSettingDTO deactivateAcdmYear([FromBody] FeeTermWiseRebateSettingDTO id)
        {
            // id = 12;
            return _feegroupHeadpage.deactivate(id);
        }



    }
}
