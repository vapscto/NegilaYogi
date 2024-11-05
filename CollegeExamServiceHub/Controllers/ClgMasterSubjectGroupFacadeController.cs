using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeExamServiceHub.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College;
using PreadmissionDTOs.com.vaps.College.Exam;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeExamServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class ClgMasterSubjectGroupFacadeController : Controller
    {
        public ClgMasterSubjectGroupInterface inter;

        public ClgMasterSubjectGroupFacadeController(ClgMasterSubjectGroupInterface obj)
        {
            inter = obj;
        }

        [Route("getdetails/{id:int}")]
        public MasterSubjectGroupDTO getorgdet(int id)
        {
            return inter.getdetails(id);
        }
        [Route("savedetail")]
        public MasterSubjectGroupDTO Post([FromBody] MasterSubjectGroupDTO org)
        {
            return inter.savedetail(org);
        }
        [Route("deactivate")]
        public MasterSubjectGroupDTO deactivate([FromBody] MasterSubjectGroupDTO org)
        {
            return inter.deactivate(org);
        }
        [Route("getalldetailsviewrecords/{id:int}")]
        //[Route("getenquirycontroller")]
        public Exm_Col_Master_Group_SubjectsDTO getalldetailsviewrecords(int id)
        {
            return inter.getalldetailsviewrecords(id);
        }

        [Route("getpagedetails/{id:int}")]
        public MasterSubjectGroupDTO getpagedetails(int id)
        {
            return inter.getpageedit(id);
        }


        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public MasterSubjectGroupDTO Deleterec(int id)
        {
            return inter.deleterec(id);
        }
    }
}
