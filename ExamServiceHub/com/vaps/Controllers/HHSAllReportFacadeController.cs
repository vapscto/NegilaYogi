
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
    public class HHSAllReportFacadeController : Controller
    {
        public HHSAllReportInterface _HHSAllReport;

        public HHSAllReportFacadeController(HHSAllReportInterface data)
        {
            _HHSAllReport = data;
        }


        [Route("Getdetails")]
        public async Task<HHSAllReportDTO> Getdetails([FromBody]HHSAllReportDTO data)//int IVRMM_Id
        {

            return await _HHSAllReport.Getdetails(data);
           
        }


        [Route("savedetails")]
        public async Task<HHSAllReportDTO> savedetails([FromBody] HHSAllReportDTO data)
        {
            return await _HHSAllReport.savedetails(data);
        }

        [Route("yearchange")]
        public HHSAllReportDTO yearchange([FromBody]HHSAllReportDTO data)
        {
            return _HHSAllReport.yearchange(data);
        }
        [Route("classchange")]
        public HHSAllReportDTO classchange([FromBody]HHSAllReportDTO data)
        {
            return _HHSAllReport.classchange(data);
        }
        [Route("sectionchange")]
        public HHSAllReportDTO sectionchange([FromBody]HHSAllReportDTO data)
        {
            return _HHSAllReport.sectionchange(data);
        }
        [Route("getbbkvreport")]
        public async Task<HHSAllReportDTO> getbbkvreport([FromBody] HHSAllReportDTO data)
        {
            return await _HHSAllReport.getbbkvreport(data);
        }   
    }
}
