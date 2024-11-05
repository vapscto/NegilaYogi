using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using CollegeServiceHub.Interface;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class MasterCourseFacadeController : Controller
    {
        public MasterCourseInterface _MsCInter;

        public MasterCourseFacadeController(MasterCourseInterface scadm)
        {
            _MsCInter = scadm;
        }       

        [HttpPost]
       [Route("Savedetails")]
        public MasterCourseDTO Savedetails([FromBody]MasterCourseDTO id)
        {
            return _MsCInter.Savedetails(id);
        }
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public MasterCourseDTO getalldetails(int id)
        {
            return _MsCInter.getalldetails(id);
        }

        [HttpPost]
        [Route("Deletedetails")]
        public MasterCourseDTO Deletedetails([FromBody]MasterCourseDTO id)
        {
            return _MsCInter.Deletedetails(id);
        }
        [Route("getOrder")]
        public MasterCourseDTO getOrder([FromBody]MasterCourseDTO id)
        {
            return _MsCInter.getOrder(id);
        }
        [Route("EditData")]
        public MasterCourseDTO EditData([FromBody]MasterCourseDTO id)
        {
            return _MsCInter.EditData(id);
        }     
        

    }
}
