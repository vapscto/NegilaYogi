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
    public class ExamWiseTermReportFacade : Controller
    {

        public ExamWiseTermReportInterface _PCReportContext;

        public ExamWiseTermReportFacade(ExamWiseTermReportInterface dt)
        {
            _PCReportContext = dt;
        }


        [Route("Getdetails")]
        public async Task<ExamWiseTermReport_DTO> Getdetails([FromBody]ExamWiseTermReport_DTO data)//int IVRMM_Id
        {
            return await _PCReportContext.Getdetails(data);
        }

        [Route("onchangeyear")]
        public ExamWiseTermReport_DTO onchangeyear([FromBody] ExamWiseTermReport_DTO data)
        {
            return _PCReportContext.onchangeyear(data);
        }

        [Route("onchangeclass")]
        public ExamWiseTermReport_DTO onchangeclass([FromBody] ExamWiseTermReport_DTO data)
        {
            return _PCReportContext.onchangeclass(data);
        }
        [Route("onchangesection")]
        public ExamWiseTermReport_DTO onchangesection([FromBody] ExamWiseTermReport_DTO data)
        {
            return _PCReportContext.onchangesection(data);
        }




    }
}
