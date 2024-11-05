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
    public class FeeMasterTermsFacade : Controller
    {
        public FeeMasterTermsInterface _feegrouppage;

        public FeeMasterTermsFacade(FeeMasterTermsInterface maspag)
        {
            _feegrouppage = maspag;
        }


        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public FeeTermDTO getpagedetails(int id)
        {
            // id = 12;
            return _feegrouppage.getpageedit(id);
        }

        // GET: api/values
        [HttpGet]
        public FeeTermDTO Get(FeeTermDTO mas)
        {
            return _feegrouppage.GetGroupSearchData(mas);
        }
        // GET api/values/5
        [Route("getdetails")]
        public FeeTermDTO getorgdet([FromBody] FeeTermDTO id)
        {
            return _feegrouppage.getdetails(id);
        }

        // POST api/values
        [HttpPost]
        public FeeTermDTO Post([FromBody] FeeTermDTO org)
        {
            return _feegrouppage.SaveGroupData(org);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public string Put(int id, [FromBody] FeeTermDTO value)
        {
            return "success";
        }


        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public FeeTermDTO Deleterec(int id)
        {
            return _feegrouppage.deleterec(id);
        }


        [HttpPost]
        [Route("deactivate")]
        public FeeTermDTO deactivateAcdmYear([FromBody] FeeTermDTO id)
        {
            // id = 12;
            return _feegrouppage.deactivate(id);
        }
        [HttpPost]
        [Route("SaveYearlyGrpdata/")]
        public FeeMasterTermHeadsDTO SaveYearlyGrpdata([FromBody] FeeMasterTermHeadsDTO reg)
        {
           
              return  _feegrouppage.SaveYearlyGroupData(reg);      

         
        }
        [HttpPost]
        [Route("savedetailfourth/")]
        public FeeMasterTermHeadsDTO savedetailfourth([FromBody] FeeMasterTermHeadsDTO reg)
        {

            return _feegrouppage.savedetailfourth(reg);


        }
        // GET api/values/5
        [Route("getdetailsY/{id:int}")]
        public FeeMasterTermHeadsDTO getorgdetY(int id)
        {
            return _feegrouppage.getdetailsY(id);
        }

        [HttpPost]
        [Route("deactivateY")]
        public FeeMasterTermHeadsDTO deactivateY([FromBody] FeeMasterTermHeadsDTO id)
        {
            // id = 12;
            return _feegrouppage.deactivateY(id);
        }

        [Route("getpagedetailsY/{id:int}")]
        //[Route("getenquirycontroller")]
        public FeeMasterTermHeadsDTO getpagedetailsY(int id)
        {
            // id = 12;
            return _feegrouppage.getpageeditY(id);
        }
        [Route("getdetailsDY/{id:int}")]
        //[Route("getenquirycontroller")]
        public FeeMasterTermFeeHeadsDueDateDTO getdetailsDY(int id)
        {
            // id = 12;
            return _feegrouppage.getdetailsDY(id);
        }
        //fourth
        [Route("getdetailsDYfourth/{id:int}")]
        public FeeMasterTermFeeHeadsDueDateDTO getdetailsDYfourth(int id)
        {
            // id = 12;
            return _feegrouppage.getdetailsDYfourth(id);
        }

        [Route("deletedetailsY")]
        public FeeMasterTermHeadsDTO DeleterecY([FromBody] FeeMasterTermHeadsDTO data)
        {
            return _feegrouppage.deleterecY(data);
        }
        [Route("DeleteYss")]
        public FeeMasterTermHeadsDTO DeleteYss([FromBody] FeeMasterTermHeadsDTO data)
        {
            return _feegrouppage.DeleteYss(data);
        }

        [Route("Getduedates/")]
        public FeeMasterTermHeadsDTO[] Getduedates([FromBody] FeeMasterTermHeadsDTO masterMDT)
        {
            // return _reg.getregdata(reg);
            return _feegrouppage.Getduedates(masterMDT);
        }

        // POST api/values
        [HttpPost]
        [Route("savedetailDDD/")]
        public FeeMasterTermHeadsDTO savedetailDDD([FromBody] FeeMasterTermHeadsDTO org)
        {
            return _feegrouppage.savedetailDDD(org);
        }
        [HttpDelete]
        [Route("deletepagesthird/{id:int}")]
        public FeeMasterTermFeeHeadsDueDateDTO deletepagesthird(int id)
        {
            return _feegrouppage.deletepagesthird(id);
        }
    }
}
