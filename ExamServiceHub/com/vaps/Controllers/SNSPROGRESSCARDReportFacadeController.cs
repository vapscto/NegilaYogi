
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
    public class SNSPROGRESSCARDReportFacadeController : Controller
    {
        public SNSPROGRESSCARDReportInterface _HHSAllReport;

        public SNSPROGRESSCARDReportFacadeController(SNSPROGRESSCARDReportInterface data)
        {
            _HHSAllReport = data;
        }


        [Route("Getdetails")]
        public async Task<SNSPROGRESSCARDReportDTO> Getdetails([FromBody]SNSPROGRESSCARDReportDTO data)//int IVRMM_Id
        {

            return await _HHSAllReport.Getdetails(data);
           
        }


        [Route("savedetails")]
        public async Task<SNSPROGRESSCARDReportDTO> savedetails([FromBody] SNSPROGRESSCARDReportDTO data)
        {
            return await _HHSAllReport.savedetails(data);
        }

        [Route("yearchange")]
        public SNSPROGRESSCARDReportDTO yearchange([FromBody]SNSPROGRESSCARDReportDTO data)
        {
            return _HHSAllReport.yearchange(data);
        }
        [Route("classchange")]
        public SNSPROGRESSCARDReportDTO classchange([FromBody]SNSPROGRESSCARDReportDTO data)
        {
            return _HHSAllReport.classchange(data);
        }
        [Route("sectionchange")]
        public SNSPROGRESSCARDReportDTO sectionchange([FromBody]SNSPROGRESSCARDReportDTO data)
        {
            return _HHSAllReport.sectionchange(data);
        }



    }
}
