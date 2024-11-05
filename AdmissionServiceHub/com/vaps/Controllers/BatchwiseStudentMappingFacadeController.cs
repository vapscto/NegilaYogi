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
    public class BatchwiseStudentMappingFacadeController : Controller
    {
        public BatchwiseStudentMappingInterface _enq;
        public BatchwiseStudentMappingFacadeController(BatchwiseStudentMappingInterface Instit)
        {
            _enq = Instit;
        }

        // POST api/values
        [HttpPost]
        public AdmSchoolAttendanceSubjectBatchDTO saveSubjectBatch([FromBody]AdmSchoolAttendanceSubjectBatchDTO sct)
        {
            return _enq.saveAdmSchoolAttendanceSubjectBatch(sct);
        }

        [Route("GetstudentdetailsbyYearandInstitute")]
        public AdmSchoolAttendanceSubjectBatchDTO GetstudentdetailsbyYearandclass([FromBody]AdmSchoolAttendanceSubjectBatchDTO sct)
        {
            return _enq.GetDropdowndetailsbyYearandInstitute(sct);
        }
        [Route("getbatchwisestdlist")]
        public AdmSchoolAttendanceSubjectBatchDTO getbatchwisestdlist([FromBody] AdmSchoolAttendanceSubjectBatchDTO data)
        {
            return _enq.getbatchwisestdlist(data);
        }
        [Route("getbatchname")]
        public AdmSchoolAttendanceSubjectBatchDTO getbatchname([FromBody] AdmSchoolAttendanceSubjectBatchDTO data)
        {
            return _enq.getbatchname(data);
        }
        

    }
}
