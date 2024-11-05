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
    public class FeegroupFacade : Controller
    {
        public FeeGroupInterface _feegrouppage;

        public FeegroupFacade(FeeGroupInterface maspag)
        {
            _feegrouppage = maspag;
        }

     
        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public FeeGroupDTO getpagedetails(int id)
        {
            // id = 12;
            return _feegrouppage.getpageedit(id);
        }

        // GET: api/values
        [HttpGet]
        public FeeGroupDTO Get(FeeGroupDTO mas)
        {
            return _feegrouppage.GetGroupSearchData(mas);
        }

        // GET api/values/5
        [Route("getdetails")]
        public FeeGroupDTO getorgdet([FromBody] FeeGroupDTO data)
        {
            return _feegrouppage.getdetails(data);
        }

        // POST api/values
        [HttpPost]
        public FeeGroupDTO Post([FromBody] FeeGroupDTO org)
        {
            return _feegrouppage.SaveGroupData(org);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public string Put(int id, [FromBody] FeeGroupDTO value)
        {
            return "success";
        }

      
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public FeeGroupDTO Deleterec(int id)
        {
            return _feegrouppage.deleterec(id);
        }


        [HttpPost]
        [Route("deactivate")]
        public FeeGroupDTO deactivateAcdmYear([FromBody] FeeGroupDTO id)
        {
            // id = 12;
            return _feegrouppage.deactivate(id);
        }

        //for fee  

        [Route("yearsbind")]
        public Task<FeeGroupDTO> Gets([FromBody] FeeGroupDTO enqo)
        {
            return _feegrouppage.getIndependentDropDowns(enqo);
        }



        //[Route("arraytemp")]
        //public Task<FeeYearlyGroupDTO> Gets([FromBody] FeeYearlyGroupDTO enqo)
        //{
        //    return _feegrouppage.getIndependentDrop(enqo);
        //}
        //[HttpPost]
        //[Route("SaveYearlyGrpdata")]
        //public FeeYearlyGroupDTO Post([FromBody] FeeYearlyGroupDTO org)
        //{
        //    return _feegrouppage.SaveYearlyGroupData(org);
        //}
        [HttpPost]
        [Route("SaveYearlyGrpdata/")]
        public FeeYearlyGroupDTO SaveYearlyGrpdata([FromBody] FeeYearlyGroupDTO reg)
        {
          //  string str = "false";
            for (int i = 0; i < reg.TempararyArrayList.Length; i++)
            {
                int Id = Convert.ToInt32(reg.TempararyArrayList[i].FMG_Id);

                
                reg.FMG_Id = Id;


                _feegrouppage.SaveYearlyGroupData(Id, reg);
            }

            return reg;  
        }
      
        // GET api/values/5
        [Route("getdetailsY/{id:int}")]
        public FeeYearlyGroupDTO getorgdetY(int id)
        {
            return _feegrouppage.getdetailsY(id);
        }

        [HttpPost]
        [Route("deactivateY")]
        public FeeYearlyGroupDTO deactivateY([FromBody] FeeYearlyGroupDTO id)
        {
            // id = 12;
            return _feegrouppage.deactivateY(id);
        }

        [Route("getpagedetailsY/{id:int}")]
        //[Route("getenquirycontroller")]
        public FeeYearlyGroupDTO getpagedetailsY(int id)
        {
            // id = 12;
            return _feegrouppage.getpageeditY(id);
        }

        [HttpDelete]
        [Route("deletedetailsY/{id:int}")]
        public FeeYearlyGroupDTO DeleterecY(int id)
        {
            return _feegrouppage.deleterecY(id);
        }

        [Route("selectacademicyear")]
        public FeeYearlyGroupDTO selectacade([FromBody] FeeYearlyGroupDTO data)
        {
            return _feegrouppage.selectacade(data);
        }
        //extra
        [HttpPost]
        [Route("savedataFTally")]
        public Fee_FeeGroup_CompanyMappingDTO savedataFTally([FromBody] Fee_FeeGroup_CompanyMappingDTO data)
        {
            return _feegrouppage.savedataFTally(data);
        }
        [Route("deletedataYYY")]
        public Fee_FeeGroup_CompanyMappingDTO deletedataYYY([FromBody] Fee_FeeGroup_CompanyMappingDTO data)
        {
            return _feegrouppage.deletedataYYY(data);
        }
    }
}
