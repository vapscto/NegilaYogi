using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PortalHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
using PortalHub.com.vaps.Student.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Student;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Student.Controllers
{
    [Route("api/[controller]")]
    public class JSHSPortal_StudentReportsFacadeController : Controller
    {
        public JSHSPortal_StudentReportsInterface _interface;

        public JSHSPortal_StudentReportsFacadeController(JSHSPortal_StudentReportsInterface _inter)
        {
            _interface = _inter;
        }

       

        [Route("Getdetails_IT")]
        public JSHSPortal_StudentReportsDTO Getdetails_IT([FromBody]JSHSPortal_StudentReportsDTO data)
        {           
            return _interface.Getdetails_IT(data);
        }

        [Route("get_Terms_IT")]
        public JSHSPortal_StudentReportsDTO get_Terms_IT([FromBody]JSHSPortal_StudentReportsDTO data)
        {
            return _interface.get_Terms_IT(data);
        }
        [Route("get_reportdetails_IT")]
        public JSHSPortal_StudentReportsDTO get_reportdetails_IT([FromBody]JSHSPortal_StudentReportsDTO data)
        {
            return _interface.get_reportdetails_IT(data);
        }
        [Route("get_Exam_grade_pc")]
        public JSHSPortal_StudentReportsDTO get_Exam_grade_pc([FromBody]JSHSPortal_StudentReportsDTO data)
        {
            return _interface.get_Exam_grade_pc(data);
        }
        [Route("saveddata_pc")]
        public Task<JSHSPortal_ProgressCardReportDTO> saveddata_pc([FromBody] JSHSPortal_ProgressCardReportDTO data)
        {
            return _interface.saveddata_pc(data);
        }

       
    }
}
