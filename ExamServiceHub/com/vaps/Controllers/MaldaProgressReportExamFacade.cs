using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MaldaProgressReportExamFacade : Controller
    {
        public MaldaProgressReportExamInterface _PCReportContext;

        public MaldaProgressReportExamFacade(MaldaProgressReportExamInterface dt)
        {
            _PCReportContext = dt;
        }


        [Route("Getdetails")]
        public async Task<MaldaProgressReportExam_DTO> Getdetails([FromBody]MaldaProgressReportExam_DTO data)//int IVRMM_Id
        {
            return await _PCReportContext.Getdetails(data);
        }


        [Route("savedetails")]
        public async Task<MaldaProgressReportExam_DTO> savedetails([FromBody] MaldaProgressReportExam_DTO data)
        {
            return await _PCReportContext.savedetails(data);
        }

        [Route("onchangeyear")]
        public MaldaProgressReportExam_DTO onchangeyear([FromBody] MaldaProgressReportExam_DTO data)
        {
            return _PCReportContext.onchangeyear(data);
        }

        [Route("onchangeclass")]
        public MaldaProgressReportExam_DTO onchangeclass([FromBody] MaldaProgressReportExam_DTO data)
        {
            return _PCReportContext.onchangeclass(data);
        }
        [Route("onchangesection")]
        public MaldaProgressReportExam_DTO onchangesection([FromBody] MaldaProgressReportExam_DTO data)
        {
            return _PCReportContext.onchangesection(data);
        }
        [Route("getreportpromotion")]
        public MaldaProgressReportExam_DTO getreportpromotion([FromBody] MaldaProgressReportExam_DTO data)
        {
            return _PCReportContext.getreportpromotion(data);
        }
        [Route("ixpromotionreport")]
        public MaldaProgressReportExam_DTO ixpromotionreport([FromBody] MaldaProgressReportExam_DTO data)
        {
            return _PCReportContext.ixpromotionreport(data);
        }
    }
}
