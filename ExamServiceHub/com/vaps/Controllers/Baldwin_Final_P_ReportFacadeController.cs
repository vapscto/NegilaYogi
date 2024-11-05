using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
using ExamServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class Baldwin_Final_P_ReportFacadeController : Controller
    {
        Baldwin_Final_P_ReportInterface inter_fr;
        public  Baldwin_Final_P_ReportFacadeController(Baldwin_Final_P_ReportInterface inter)
        {
            inter_fr = inter;
        }
       [HttpPost]
       [Route("Getdetails")]
       public Baldwin_Final_P_ReportDTO Getdetails([FromBody] Baldwin_Final_P_ReportDTO data)
        {
            return inter_fr.Getdetails(data);
        }
        [Route("get_classes")]
        public Baldwin_Final_P_ReportDTO get_classes([FromBody] Baldwin_Final_P_ReportDTO data)
        {
            return inter_fr.get_classes(data);
        }
        [Route("get_sections")]
        public Baldwin_Final_P_ReportDTO get_sections([FromBody] Baldwin_Final_P_ReportDTO data)
        {
            return inter_fr.get_sections(data);
        }
        [Route("get_students")]
        public Baldwin_Final_P_ReportDTO get_students([FromBody] Baldwin_Final_P_ReportDTO data)
        {
            return inter_fr.get_students(data);
        }
        [Route("get_report")]
        public Baldwin_Final_P_ReportDTO get_report([FromBody] Baldwin_Final_P_ReportDTO data)
        {
            return inter_fr.get_report(data);
        }
    }
}
