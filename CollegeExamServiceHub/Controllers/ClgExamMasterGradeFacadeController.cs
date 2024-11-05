using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeExamServiceHub.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeExamServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class ClgExamMasterGradeFacadeController : Controller
    {
        public ClgExamMasterGradeInterface inter;

        public ClgExamMasterGradeFacadeController(ClgExamMasterGradeInterface obj)
        {
            inter = obj;
        }
        [Route("getdetails/{id:int}")]
        public MasterExamGradeDTO getorgdet(int id)
        {
            return inter.getdetails(id);
        }

        [Route("savedetail")]
        public MasterExamGradeDTO Post([FromBody] MasterExamGradeDTO org)
        {
            return inter.savedetail(org);
        }

        [Route("deactivate")]
        public MasterExamGradeDTO deactivate([FromBody] MasterExamGradeDTO org)
        {
            return inter.deactivate(org);
        }

        [Route("getalldetailsviewrecords/{id:int}")]
        public MasterExamGradeDTO getalldetailsviewrecords(int id)
        {
            return inter.getalldetailsviewrecords(id);
        }

        [Route("getpagedetails/{id:int}")]
        public MasterExamGradeDTO getpagedetails(int id)
        {
            return inter.getpageedit(id);
        }

        [Route("deletedetails/{id:int}")]
        public MasterExamGradeDTO Deleterec(int id)
        {
            return inter.deleterec(id);
        }
    }
}
