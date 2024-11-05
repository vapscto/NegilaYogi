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
    public class FeePDCFacade : Controller
    {
        public FeePDCInterface _feegroupHeadpage;

        public FeePDCFacade(FeePDCInterface maspag)
        {
            _feegroupHeadpage = maspag;
        }

        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public FeePDCDTO getpagedetails(int id)
        {
            // id = 12;
            return _feegroupHeadpage.getpageedit(id);
        }

        // GET: api/values
        [HttpGet]
        public FeePDCDTO Get(FeePDCDTO mas)
        {
            return _feegroupHeadpage.GetGroupSearchData(mas);
        }




        // GET api/values/5
        [Route("getdetails/{id:int}")]
        public FeePDCDTO getorgdet(int id)
        {
            return _feegroupHeadpage.getdetails(id);
        }

        // POST api/values
        [HttpPost]
        public FeePDCDTO Post([FromBody] FeePDCDTO org)
        {
            return _feegroupHeadpage.SaveGroupData(org);
        }

        [HttpPost]
        [Route("deactivate")]
        public FeePDCDTO deactivateAcdmYear([FromBody] FeePDCDTO id)
        {
            // id = 12;
            return _feegroupHeadpage.deactivate(id);
        }
        [Route("PDCRemainder")]
        public FeePDCDTO PDCRemainder([FromBody] FeePDCDTO data)
        {
            return _feegroupHeadpage.PDCRemainder(data);
        }


    }
}
