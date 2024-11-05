
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
    public class BaldwinAllReportFacadeController : Controller
    {
        public BaldwinAllReportInterface _BaldwinAllReport;

        public BaldwinAllReportFacadeController(BaldwinAllReportInterface data)
        {
            _BaldwinAllReport = data;
        }


        [Route("Getdetails")]
        public async Task<BaldwinAllReportDTO> Getdetails([FromBody]BaldwinAllReportDTO data)//int IVRMM_Id
        {

            return await _BaldwinAllReport.Getdetails(data);
           
        }

      
        [Route("savedetails")]
        public async Task<BaldwinAllReportDTO> savedetails([FromBody] BaldwinAllReportDTO data)
        {
            return await _BaldwinAllReport.savedetails(data);
        }
       
         
       

    }
}
