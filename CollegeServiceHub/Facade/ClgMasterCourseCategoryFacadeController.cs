using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using CollegeServiceHub.Interface;

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class ClgMasterCourseCategoryFacadeController : Controller
    {
        public ClgMasterCourseCategoryMapInterface _MsCInter;

        public ClgMasterCourseCategoryFacadeController(ClgMasterCourseCategoryMapInterface scadm)
        {
            _MsCInter = scadm;
        }
        

        [HttpPost]
        [Route("Savedetails")]
        public ClgMasterCourseCategoryMapDTO Savedetails([FromBody]ClgMasterCourseCategoryMapDTO id)
        {
            return _MsCInter.Savedetails(id);
        }
       
        [Route("getalldetails")]
        public ClgMasterCourseCategoryMapDTO getalldetails([FromBody] ClgMasterCourseCategoryMapDTO id)
        {
            return _MsCInter.getalldetails(id);
        }

        [HttpPost]
        [Route("deactive")]
        public ClgMasterCourseCategoryMapDTO deactive([FromBody]ClgMasterCourseCategoryMapDTO id)
        {
            return _MsCInter.deactive(id);
        }
    }
}
