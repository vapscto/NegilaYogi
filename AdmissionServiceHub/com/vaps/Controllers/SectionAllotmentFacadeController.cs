using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using AdmissionServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vapstech.controller
{
    [Route("api/[controller]")]
    public class SectionAllotmentFacadeController : Controller
    {
        public SectionAllotmentInterface _enq;
        public SectionAllotmentFacadeController(SectionAllotmentInterface Instit)
        {
            _enq = Instit;
        }

        [Route("getAllDetails")]
        public SchoolYearWiseStudentDTO Getdata([FromBody]SchoolYearWiseStudentDTO sct)
        {
            return _enq.GetDropDownList(sct);
        }
        
        [HttpGet]
        [Route("getStudentdetailsByYear/{id:int}")]
        public SchoolYearWiseStudentDTO getStudentdetailsByYear(long id)
        {
            return _enq.GetStudentListByYear(id);
        }
        
        [HttpPost]
        public SchoolYearWiseStudentDTO saveSctionAllotment([FromBody]SchoolYearWiseStudentDTO sct)
        {
            return _enq.saveSctionAllotment(sct);
        }

        [Route("GetstudentdetailsbyYearandclass")]
        public SchoolYearWiseStudentDTO GetstudentdetailsbyYearandclass([FromBody]SchoolYearWiseStudentDTO sct)
        {
            return _enq.GetstudentdetailsbyYearandclass(sct);
        }

        [Route("GetStudentListByURN")]
        public SchoolYearWiseStudentDTO GetStudentListByURN([FromBody] SchoolYearWiseStudentDTO data)
        {
            return _enq.GetStudentListByURN(data);
        }

        [Route("GetStudentListByURNsave")]
        public Student_Update_RollNumber GetStudentListByURNsave([FromBody]Student_Update_RollNumber data)
        {
            return _enq.GetStudentListByURNsave(data);
        }

        // Change Class

        [Route("GetChangeClassDetails")]
        public SchoolYearWiseStudentDTO GetChangeClassDetails([FromBody]SchoolYearWiseStudentDTO sct)
        {
            return _enq.GetChangeClassDetails(sct);
        }

        [Route("GetStudentListByYearCLS")]
        public SchoolYearWiseStudentDTO GetStudentListByYearCLS([FromBody] SchoolYearWiseStudentDTO data)
        {
            return _enq.GetStudentListByYearCLS(data);
        }

        [Route("onstudentnamechange")]
        public SchoolYearWiseStudentDTO onstudentnamechange([FromBody]SchoolYearWiseStudentDTO data)
        {
            return _enq.onstudentnamechange(data);
        }

        [Route("DeleteFeeMapping")]
        public SchoolYearWiseStudentDTO DeleteFeeMapping([FromBody]SchoolYearWiseStudentDTO data)
        {
            return _enq.DeleteFeeMapping(data);
        }

        [Route("SaveClassChange")]
        public SchoolYearWiseStudentDTO SaveClassChange([FromBody]SchoolYearWiseStudentDTO data)
        {
            return _enq.SaveClassChange(data);
        }
        [Route("SaveClassFeeChange")]
        public SchoolYearWiseStudentDTO SaveClassFeeChange([FromBody]SchoolYearWiseStudentDTO data)
        {
            return _enq.SaveClassFeeChange(data);
        }
    }
}
