using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeFeeService.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.College.Fee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class FeeGroupClgFacadeController : Controller
    {
        public FeeGroupClgInterface _feegrouppage;

        public FeeGroupClgFacadeController(FeeGroupClgInterface maspag)
        {
            _feegrouppage = maspag;
        }

        [HttpGet]
        public FeeGroupClgDTO Get(FeeGroupClgDTO mas)
        {
            return _feegrouppage.GetGroupSearchData(mas);
        }
        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public FeeGroupClgDTO getpagedetails(int id)
        {
            // id = 12;
            return _feegrouppage.getpageedit(id);
        }       

        [Route("getdetailsY/{id:int}")]
        public FeeYearlyGroupClgDTO getorgdetY(int id)
        {
            return _feegrouppage.getdetailsY(id);
        }
        [Route("getpagedetailsY/{id:int}")]
        //[Route("getenquirycontroller")]
        public FeeYearlyGroupClgDTO getpagedetailsY(int id)
        {
            // id = 12;
            return _feegrouppage.getpageeditY(id);
        }
        // GET: api/values
        //  [HttpGet]


        // GET api/values/5
        [HttpPost]
        public FeeGroupClgDTO Post([FromBody] FeeGroupClgDTO org)
        {
            return _feegrouppage.SaveGroupData(org);
        }
        [Route("getdetails")]
        public FeeGroupClgDTO getorgdet([FromBody] FeeGroupClgDTO data)
        {
            return _feegrouppage.getdetails(data);
        }
        
        [Route("deactivate")]
        public FeeGroupClgDTO deactivateAcdmYear([FromBody] FeeGroupClgDTO id)
        {
            // id = 12;
            return _feegrouppage.deactivate(id);
        }
        [Route("yearsbind")]
        public Task<FeeGroupClgDTO> Gets([FromBody] FeeGroupClgDTO enqo)
        {
            return _feegrouppage.getIndependentDropDowns(enqo);
        }
        [Route("SaveYearlyGrpdata/")]
        public FeeYearlyGroupClgDTO SaveYearlyGrpdata([FromBody] FeeYearlyGroupClgDTO reg)
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
        [Route("deactivateY")]
        public FeeYearlyGroupClgDTO deactivateY([FromBody] FeeYearlyGroupClgDTO id)
        {
            // id = 12;
            return _feegrouppage.deactivateY(id);
        }
        [Route("selectacademicyear")]
        public FeeYearlyGroupClgDTO selectacade([FromBody] FeeYearlyGroupClgDTO data)
        {
            return _feegrouppage.selectacade(data);
        }
        // POST api/values
        //  [HttpPost]


        // PUT api/values/5
        [HttpPut("{id}")]
        public string Put(int id, [FromBody] FeeGroupClgDTO value)
        {
            return "success";
        }


        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public FeeGroupClgDTO Deleterec(int id)
        {
            return _feegrouppage.deleterec(id);
        }
        [Route("deletedetailsY/{id:int}")]
        public FeeYearlyGroupClgDTO DeleterecY(int id)
        {
            return _feegrouppage.deleterecY(id);
        }

        //  [HttpPost]


        //for fee  





        //[Route("arraytemp")]
        //public Task<FeeYearlyGroupClgDTO> Gets([FromBody] FeeYearlyGroupClgDTO enqo)
        //{
        //    return _feegrouppage.getIndependentDrop(enqo);
        //}
        //[HttpPost]
        //[Route("SaveYearlyGrpdata")]
        //public FeeYearlyGroupClgDTO Post([FromBody] FeeYearlyGroupClgDTO org)
        //{
        //    return _feegrouppage.SaveYearlyGroupData(org);
        //}
        // [HttpPost]


        // GET api/values/5


        // [HttpPost]




        // [HttpDelete]




    }
}
