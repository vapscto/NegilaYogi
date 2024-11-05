
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
//using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class CumulativeReportFacadeController : Controller
    {
        public CumulativeReportInterface _CumulativeReport;

        public CumulativeReportFacadeController(CumulativeReportInterface data)
        {
            _CumulativeReport = data;
        }


        [Route("Getdetails")]
        public CumulativeReportDTO Getdetails([FromBody]CumulativeReportDTO data)//int IVRMM_Id
        {

            return _CumulativeReport.Getdetails(data);

        }

        [Route("editdetails/{id:int}")]
        public CumulativeReportDTO editdetails(int ID)
        {
            return _CumulativeReport.editdetails(ID);
        }

        [Route("validateordernumber")]
        public CumulativeReportDTO validateordernumber([FromBody] CumulativeReportDTO data)
        {
            return _CumulativeReport.validateordernumber(data);
        }

        [Route("savedetails")]
        public async Task<CumulativeReportDTO> savedetails([FromBody] CumulativeReportDTO data)
        {
            return await _CumulativeReport.savedetails(data);
        }

        [Route("deactivate")]
        public CumulativeReportDTO deactivate([FromBody] CumulativeReportDTO data)
        {
            return _CumulativeReport.deactivate(data);
        }
        [Route("onchangeyear")]
        public CumulativeReportDTO onchangeyear([FromBody] CumulativeReportDTO data)
        {
            return _CumulativeReport.onchangeyear(data);
        }

        [Route("onchangeclass")]
        public CumulativeReportDTO onchangeclass([FromBody] CumulativeReportDTO data)
        {
            return _CumulativeReport.onchangeclass(data);
        }
        [Route("onchangesection")]
        public CumulativeReportDTO onchangesection([FromBody] CumulativeReportDTO data)
        {
            return _CumulativeReport.onchangesection(data);
        }

    }
}
