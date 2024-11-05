using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fee;
using CollegeFeeService.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class FeeHeadClgFacadeController : Controller
    {
        public FeeHeadClgInterface _feegroupHeadpage;

        public FeeHeadClgFacadeController(FeeHeadClgInterface maspag)
        {
            _feegroupHeadpage = maspag;
        }

        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public FeeHeadClgDTO getpagedetails(int id)
        {
            return _feegroupHeadpage.getpageedit(id);
        }
        [HttpGet]
        public FeeHeadClgDTO Get(FeeHeadClgDTO mas)
        {
            return _feegroupHeadpage.GetGroupSearchData(mas);
        }


        [Route("changeorderData")]
        public FeeHeadClgDTO changeorderData([FromBody] FeeHeadClgDTO mas)
        {
            return _feegroupHeadpage.changeorderData(mas);
        }

        // GET api/values/5
        [Route("getdetails/{id:int}")]
        public FeeHeadClgDTO getorgdet(int id)
        {
            return _feegroupHeadpage.getdetails(id);
        }

        // POST api/values
        [HttpPost]
        public FeeHeadClgDTO Post([FromBody] FeeHeadClgDTO org)
        {
            return _feegroupHeadpage.SaveGroupData(org);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public string Put(int id, [FromBody] FeeHeadClgDTO value)
        {
            return "success";
        }


        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public FeeHeadClgDTO Deleterec(int id)
        {
            return _feegroupHeadpage.deleterec(id);
        }


        [HttpPost]
        [Route("deactivate")]
        public FeeHeadClgDTO deactivateAcdmYear([FromBody] FeeHeadClgDTO id)
        {
            // id = 12;
            return _feegroupHeadpage.deactivate(id);
        }


        [Route("getallbankdetails")]
        public FeeHeadClgDTO getallbankdetails([FromBody] FeeHeadClgDTO data)
        {
            return _feegroupHeadpage.getallbankdetails(data);
        }

        [Route("savedata")]
        public FeeHeadClgDTO savedata([FromBody] FeeHeadClgDTO data)
        {
            return _feegroupHeadpage.savedata(data);
        }
        [Route("edit")]
        public FeeHeadClgDTO edit([FromBody] FeeHeadClgDTO data)
        {
            return _feegroupHeadpage.edit(data);
        }
        [Route("activedeactive")]
        public FeeHeadClgDTO activedeactive([FromBody] FeeHeadClgDTO data)
        {
            return _feegroupHeadpage.activedeactive(data);
        }
    }
}
