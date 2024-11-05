using Admission.com.vapstech.Delegates;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admission.com.vapstech.controller
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class BatchwiseStudentMappingController : Controller
    {


        BatchwiseStudentMappingDelegate Delegate = new BatchwiseStudentMappingDelegate();
        // POST api/values
        [HttpPost]
        public AdmSchoolAttendanceSubjectBatchDTO SaveBatchwiseStudentMapping([FromBody] AdmSchoolAttendanceSubjectBatchDTO Ins)
        {
            Ins.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            // Ins.AMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return Delegate.SaveDetails(Ins);
        }

        [Route("GetstudentdetailsbyYearandInstitute1/{id:int}")]
        public AdmSchoolAttendanceSubjectBatchDTO GetstudentdetailsbyYearandInstitute1(int id)
        {
            AdmSchoolAttendanceSubjectBatchDTO yearclass = new AdmSchoolAttendanceSubjectBatchDTO();
            yearclass.FormType = "onload";
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            // yearclass.AMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return Delegate.GetstudentdetailsbyYearandInstitute(yearclass);
        }

        [Route("GetstudentdetailsbyYearandInstitute")]
        public AdmSchoolAttendanceSubjectBatchDTO GetStudentListByYearAndCLass_CS([FromBody] AdmSchoolAttendanceSubjectBatchDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            // yearclass.AMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return Delegate.GetstudentdetailsbyYearandInstitute(yearclass);
        }
        [Route("getbatchwisestdlist")]
        public AdmSchoolAttendanceSubjectBatchDTO getbatchwisestdlist([FromBody] AdmSchoolAttendanceSubjectBatchDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return Delegate.getbatchwisestdlist(data);
        }
        [Route("getbatchname")]
        public AdmSchoolAttendanceSubjectBatchDTO getbatchname([FromBody] AdmSchoolAttendanceSubjectBatchDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return Delegate.getbatchname(data);
        }
    }
}
