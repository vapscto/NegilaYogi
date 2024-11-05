using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using TimeTableServiceHub.Interfaces;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTableServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class CLGCategoryMappingFacadeController : Controller
    {
        public CLGCategoryMappingInterface _acd;
        public CLGCategoryMappingFacadeController(CLGCategoryMappingInterface acdm)
        {
            _acd = acdm;
        }
        [HttpPost]
        [Route("getalldetails")]
        public CLGCategoryMappingDTO getalldetails([FromBody]CLGCategoryMappingDTO data)
        {
            return _acd.getalldetails(data);
        }

       
        [HttpPost]
        [Route("getBranch")]
        public CLGCategoryMappingDTO getBranch([FromBody]CLGCategoryMappingDTO data)
        {
          
            return _acd.getBranch(data);
          
        }

        [HttpPost]
        [Route("savedetails")]
        public CLGCategoryMappingDTO savedetails([FromBody]CLGCategoryMappingDTO data)
        {
          
            return _acd.savedetails(data);
          
        }
        //// PUT api/values/5
        [HttpPut]
        public string Put([FromBody]OrganisationDTO org)
        {
            return "success";
        }



        [Route("deletedetails")]
        public CLGCategoryMappingDTO Deleterec([FromBody]CLGCategoryMappingDTO data)
        {
            return _acd.deleterec(data);
        }
    }
}
