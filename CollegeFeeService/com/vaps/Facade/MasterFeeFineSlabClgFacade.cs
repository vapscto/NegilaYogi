using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fee;
using CollegeFeeService.com.vaps.interfaces;
using PreadmissionDTOs.com.vaps.Fees;
using CollegeFeeService.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class MasterFeeFineSlabClgFacade : Controller
    {
        public MasterFeeFineSlabClgInterface _feegroupHeadpage;

        public MasterFeeFineSlabClgFacade(MasterFeeFineSlabClgInterface maspag)
        {
            _feegroupHeadpage = maspag;
        }

        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public MasterFeeFineSlabClg_DTO getpagedetails(int id)
        {
            // id = 12;
            return _feegroupHeadpage.getpageedit(id);
        }

        // GET: api/values
        [HttpGet]
        public MasterFeeFineSlabClg_DTO Get(MasterFeeFineSlabClg_DTO mas)
        {
            return _feegroupHeadpage.GetGroupSearchData(mas);
        }

        // GET api/values/5
        [Route("getdetails/{id:int}")]
        public MasterFeeFineSlabClg_DTO getorgdet(int id)
        {
            return _feegroupHeadpage.getdetails(id);
        }

        // POST api/values
        [HttpPost]
        public MasterFeeFineSlabClg_DTO Post([FromBody] MasterFeeFineSlabClg_DTO org)
        {
            return _feegroupHeadpage.SaveGroupData(org);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public string Put(int id, [FromBody] MasterFeeFineSlabClg_DTO value)
        {
            return "success";
        }


        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public MasterFeeFineSlabClg_DTO Deleterec(int id)
        {
            return _feegroupHeadpage.deleterec(id);
        }


        [HttpPost]
        [Route("deactivate")]
        public MasterFeeFineSlabClg_DTO deactivateAcdmYear([FromBody] MasterFeeFineSlabClg_DTO id)
        {
            // id = 12;
            return _feegroupHeadpage.deactivate(id);
        }
    }
}
