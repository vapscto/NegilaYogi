using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeExamServiceHub.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeExamServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class ClgCumulativeReportFacadeController : Controller
    {
        public ClgCumulativeReportInterface _CumulativeReport;

        public ClgCumulativeReportFacadeController(ClgCumulativeReportInterface data)
        {
            _CumulativeReport = data;
        }

        [Route("Getdetails")]
        public ClgCumulativeReportDTO Getdetails([FromBody]ClgCumulativeReportDTO data)
        {
            return _CumulativeReport.Getdetails(data);

        }

        [Route("Getcmreport")]
        public async Task<ClgCumulativeReportDTO> Getcmreport([FromBody] ClgCumulativeReportDTO data)
        {
            return await _CumulativeReport.Getcmreport(data);
        }

        [Route("onchangeyear")]
        public ClgCumulativeReportDTO onchangeyear([FromBody] ClgCumulativeReportDTO data)
        {
            return _CumulativeReport.onchangeyear(data);
        }

        [Route("onchangecourse")]
        public ClgCumulativeReportDTO onchangecourse([FromBody] ClgCumulativeReportDTO data)
        {
            return _CumulativeReport.onchangecourse(data);
        }

        [Route("onchangebranch")]
        public ClgCumulativeReportDTO onchangebranch([FromBody] ClgCumulativeReportDTO data)
        {
            return _CumulativeReport.onchangebranch(data);
        }

        [Route("onchangesemester")]
        public ClgCumulativeReportDTO onchangesemester([FromBody] ClgCumulativeReportDTO data)
        {
            return _CumulativeReport.onchangesemester(data);
        }

        [Route("onchangesubjectscheme")]
        public ClgCumulativeReportDTO onchangesubjectscheme([FromBody] ClgCumulativeReportDTO data)
        {
            return _CumulativeReport.onchangesubjectscheme(data);
        }

        [Route("onchangeschemetype")]
        public ClgCumulativeReportDTO onchangeschemetype([FromBody] ClgCumulativeReportDTO data)
        {
            return _CumulativeReport.onchangeschemetype(data);
        }

        [Route("GetCumulativeReportFormat2")]
        public ClgCumulativeReportDTO GetCumulativeReportFormat2([FromBody] ClgCumulativeReportDTO data)
        {
            return _CumulativeReport.GetCumulativeReportFormat2(data);
        }

        [Route("GetProgresscardReport")]
        public ClgCumulativeReportDTO GetProgresscardReport([FromBody] ClgCumulativeReportDTO data)
        {
            return _CumulativeReport.GetProgresscardReport(data);
        }

        [Route("GetJNUProgressCardReport1")]
        public ClgCumulativeReportDTO GetJNUProgressCardReport1([FromBody] ClgCumulativeReportDTO data)
        {
            return _CumulativeReport.GetJNUProgressCardReport1(data);
        }
    }
}
