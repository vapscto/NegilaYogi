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
    public class ClgCourseBranchMapFacadeController : Controller
    {
        public ClgMasterCourseBranchMapInterface _MsCInter;

        public ClgCourseBranchMapFacadeController(ClgMasterCourseBranchMapInterface scadm)
        {
            _MsCInter = scadm;
        }


        [HttpPost]
        [Route("Savedetails")]
        public ClgMasterCourseBranchMapDTO Savedetails([FromBody]ClgMasterCourseBranchMapDTO id)
        {
            return _MsCInter.Savedetails(id);
        }
        [Route("getalldetails")]
        public ClgMasterCourseBranchMapDTO getalldetails([FromBody] ClgMasterCourseBranchMapDTO id)
        {
            return _MsCInter.getalldetails(id);
        }

        [HttpPost]
        [Route("Deletedetails")]
        public ClgMasterCourseBranchMapDTO Deletedetails([FromBody]ClgMasterCourseBranchMapDTO id)
        {
            return _MsCInter.Deletedetails(id);
        }
        [Route("showmodaldetails")]
        public ClgMasterCourseBranchMapDTO showmodaldetails([FromBody]ClgMasterCourseBranchMapDTO id)
        {
            return _MsCInter.showmodaldetails(id);
        }
        [Route("deactivesem")]
        public ClgMasterCourseBranchMapDTO deactivesem([FromBody]ClgMasterCourseBranchMapDTO id)
        {
            return _MsCInter.deactivesem(id);
        }
        [Route("edit")]
        public ClgMasterCourseBranchMapDTO edit([FromBody]ClgMasterCourseBranchMapDTO id)
        {
            return _MsCInter.edit(id);
        }
        
    }
}
