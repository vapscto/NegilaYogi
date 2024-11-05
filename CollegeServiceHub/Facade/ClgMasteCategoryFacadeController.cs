using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using CollegeServiceHub.Interface;
using System.IO;


namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class ClgMasterCategoryFacadeController : Controller
    {
        public ClgMasterCategoryInterface _catry;

        public ClgMasterCategoryFacadeController(ClgMasterCategoryInterface ct)
        {
            _catry = ct;
        }
        [HttpPost]
        [Route("Savedetails")]
        public ClgMasterCategoryDTO Savedetails([FromBody]ClgMasterCategoryDTO id)
        {
            return _catry.Savedetails(id);
        }
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public ClgMasterCategoryDTO getalldetails(int id)
        {
            return _catry.getalldetails(id);
        }

        [HttpPost]
        [Route("Deletedetails")]
        public ClgMasterCategoryDTO Deletedetails([FromBody]ClgMasterCategoryDTO id)
        {
            return _catry.Deletedetails(id);
        }
        [Route("deactivate")]
        public ClgMasterCategoryDTO deactivate([FromBody]ClgMasterCategoryDTO id)
        {
            return _catry.deactivate(id);
        }
        
    }
}