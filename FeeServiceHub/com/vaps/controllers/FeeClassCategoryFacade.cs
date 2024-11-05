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
    public class FeeClassCategoryFacade : Controller
    {
        public FeeClassCategoryInterface _feegrouppage;

        public FeeClassCategoryFacade(FeeClassCategoryInterface maspag)
        {
            _feegrouppage = maspag;
        }


        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public FeeClassCategoryDTO getpagedetails(int id)
        {
            // id = 12;
            return _feegrouppage.getpageedit(id);
        }

        // GET: api/values
        [HttpGet]
        public FeeClassCategoryDTO Get(FeeClassCategoryDTO mas)
        {
            return _feegrouppage.GetGroupSearchData(mas);
        }

        // GET api/values/5
        [Route("getdetails")]
        public FeeClassCategoryDTO getorgdet([FromBody] FeeClassCategoryDTO id)
        {
            return _feegrouppage.getdetails(id);
        }

        // POST api/values
        [HttpPost]
        public FeeClassCategoryDTO Post([FromBody] FeeClassCategoryDTO org)
        {
            return _feegrouppage.SaveGroupData(org);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public string Put(int id, [FromBody] FeeClassCategoryDTO value)
        {
            return "success";
        }


      
        [Route("deletedetails/{id:int}")]
        public FeeClassCategoryDTO Deleterec(int id)
        {
            return _feegrouppage.deleterec(id);
        }


        [HttpPost]
        [Route("deactivate")]
        public FeeClassCategoryDTO deactivateAcdmYear([FromBody] FeeClassCategoryDTO id)
        {
            // id = 12;
            return _feegrouppage.deactivate(id);
        }

        //for fee  

        [Route("yearsbind")]
        public Task<FeeClassCategoryDTO> Gets([FromBody] FeeClassCategoryDTO enqo)
        {
            return _feegrouppage.getIndependentDropDowns(enqo);
        }




        [HttpPost]
        [Route("SaveYearlyGrpdata/")]
        public FeeYearlyClassCategoryDTO SaveYearlyGrpdata([FromBody] FeeYearlyClassCategoryDTO reg)
        {
            //  string str = "false";
            for (int i = 0; i < reg.TempararyArrayList.Length; i++)
            {
                int Id = Convert.ToInt32(reg.TempararyArrayList[i].ASMCL_ID);
                reg.ASMCL_ID = Id;
                _feegrouppage.SaveYearlyGroupData(Id, reg);
            }

            return reg;
        }

        // GET api/values/5
        [Route("getdetailsY/{id:int}")]
        public FeeYearlyClassCategoryDTO getorgdetY(int id)
        {
            return _feegrouppage.getdetailsY(id);
        }

        [HttpPost]
        [Route("deactivateY")]
        public FeeYearlyClassCategoryDTO deactivateY([FromBody] FeeYearlyClassCategoryDTO id)
        {
            // id = 12;
            return _feegrouppage.deactivateY(id);
        }

        [Route("getpagedetailsY/{id:int}")]
        //[Route("getenquirycontroller")]
        public FeeYearlyClassCategoryDTO getpagedetailsY(int id)
        {
            // id = 12;
            return _feegrouppage.getpageeditY(id);
        }

   
        [Route("deletedetailsY/{id:int}")]
        public FeeYearlyClassCategoryDTO DeleterecY(int id)
        {
            return _feegrouppage.deleterecY(id);
        }

        [Route("loaddata")]
        public FeeYearlyClassCategoryDTO Loaddata([FromBody] FeeYearlyClassCategoryDTO data)
        {
            return _feegrouppage.loaddata(data);
        }
    }
}
