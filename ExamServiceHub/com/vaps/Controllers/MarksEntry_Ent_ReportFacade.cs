
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
    public class MarksEntry_Ent_ReportFacade : Controller
    {
        public MarksEntry_Ent_ReportInterface _BaldwinAllReport;

        public MarksEntry_Ent_ReportFacade(MarksEntry_Ent_ReportInterface data)
        {
            _BaldwinAllReport = data;
        }


        [Route("Getdetails")]
        public async Task<MarksEntry_Ent_ReportDTO> Getdetails([FromBody]MarksEntry_Ent_ReportDTO data)
        {

            return await _BaldwinAllReport.Getdetails(data);
           
        }
        
        [Route("get_report")]
        public MarksEntry_Ent_ReportDTO get_report([FromBody] MarksEntry_Ent_ReportDTO data)
        {
            return  _BaldwinAllReport.get_report(data);
        }
        //SubjectList
        [Route("SubjectList")]
        public MarksEntry_Ent_ReportDTO SubjectList([FromBody] MarksEntry_Ent_ReportDTO data)
        {
            return _BaldwinAllReport.SubjectList(data);
        }
    }
}
