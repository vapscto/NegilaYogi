using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Student.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Student;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Student.Controllers
{
    [Route("api/[controller]")]
    public class StudentProgressCardReportFacadeController : Controller
    {
        public StudentProgressCardReportInterface _interface;

        public StudentProgressCardReportFacadeController(StudentProgressCardReportInterface _inter)
        {
            _interface = _inter;
        }

        [Route("getdetails")]
        public StudentProgressCardReportDTO getdetails ([FromBody]StudentProgressCardReportDTO data)
        {
            return _interface.getdetails(data);
        }

        [Route("onchangeclass")]
        public StudentProgressCardReportDTO onchangeclass([FromBody]StudentProgressCardReportDTO data)
        {
            return _interface.onchangeclass(data);
        }

        [Route("getreport")]
        public StudentProgressCardReportDTO getreport([FromBody]StudentProgressCardReportDTO data)
        {
            return _interface.getreport(data);
        }

        // BGHS
        [Route("Bghsgetdetails")]
        public StudentProgressCardReportDTO Bghsgetdetails([FromBody]StudentProgressCardReportDTO data)
        {
            return _interface.Bghsgetdetails(data);
        }

        [Route("Bghsonchangeclass")]
        public StudentProgressCardReportDTO Bghsonchangeclass([FromBody]StudentProgressCardReportDTO data)
        {
            return _interface.Bghsonchangeclass(data);
        }

        [Route("Bghsgetreport")]
        public StudentProgressCardReportDTO Bghsgetreport([FromBody]StudentProgressCardReportDTO data)
        {
            return _interface.Bghsgetreport(data);
        }

        // Stmary
        [Route("stmarygetdetails")]
        public StudentProgressCardReportDTO stmarygetdetails([FromBody]StudentProgressCardReportDTO data)
        {
            return _interface.stmarygetdetails(data);
        }

        [Route("stmaryonchangeclass")]
        public StudentProgressCardReportDTO stmaryonchangeclass([FromBody]StudentProgressCardReportDTO data)
        {
            return _interface.stmaryonchangeclass(data);
        }

        [Route("stmarygetreport")]
        public StudentProgressCardReportDTO stmarygetreport([FromBody]StudentProgressCardReportDTO data)
        {
            return _interface.stmarygetreport(data);
        }

        //HHS
        [Route("HHSStudentProgressCardReport")]
        public StudentProgressCardReportDTO HHSStudentProgressCardReport([FromBody]StudentProgressCardReportDTO data)
        {
            return _interface.HHSStudentProgressCardReport(data);
        }

        //Stjames
        [Route("Get_Stjames_Progresscard_Report")]
        public StudentProgressCardReportDTO Get_Stjames_Progresscard_Report([FromBody]StudentProgressCardReportDTO data)
        {
            return _interface.Get_Stjames_Progresscard_Report(data);
        }

        //NDS
        [Route("NDS_Get_Progresscard_Report")]
        public StudentProgressCardReportDTO NDS_Get_Progresscard_Report([FromBody]StudentProgressCardReportDTO data)
        {
            return _interface.NDS_Get_Progresscard_Report(data);
        }

        //BCEHS
        [Route("Get_BCEHS_Progresscard_Report")]
        public StudentProgressCardReportDTO Get_BCEHS_Progresscard_Report([FromBody]StudentProgressCardReportDTO data)
        {
            return _interface.Get_BCEHS_Progresscard_Report(data);
        }

        //BIS
        [Route("BISStudentProgressCardReport")]
        public StudentProgressCardReportDTO BISStudentProgressCardReport([FromBody]StudentProgressCardReportDTO data)
        {
            return _interface.BISStudentProgressCardReport(data);
        }
    }
}
