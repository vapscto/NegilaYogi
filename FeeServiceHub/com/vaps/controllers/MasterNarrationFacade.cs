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
    public class MasterNarrationFacade : Controller
    {
        public MasterNarrationInterface _feegroupHeadpage;

        public MasterNarrationFacade(MasterNarrationInterface maspag)
        {
            _feegroupHeadpage = maspag;
        }

        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public MasterNarrationDTO getpagedetails(int id)
        {
            // id = 12;
            return _feegroupHeadpage.getpageedit(id);
        }

        // GET: api/values
        [HttpGet]
        public MasterNarrationDTO Get(MasterNarrationDTO mas)
        {
            return _feegroupHeadpage.GetGroupSearchData(mas);
        }




        // GET api/values/5
        [Route("getdetails/{id:int}")]
        public MasterNarrationDTO getorgdet(int id)
        {
            return _feegroupHeadpage.getdetails(id);
        }

        // POST api/values
        [HttpPost]
        public MasterNarrationDTO Post([FromBody] MasterNarrationDTO org)
        {
            return _feegroupHeadpage.SaveGroupData(org);
        }

        [HttpPost]
        [Route("deactivate")]
        public MasterNarrationDTO deactivateAcdmYear([FromBody] MasterNarrationDTO id)
        {
            // id = 12;
            return _feegroupHeadpage.deactivate(id);
        }
       



    }
}
