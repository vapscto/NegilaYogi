using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using WebApplication1.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class PreadmissionNoticeRegistrationReportFacade : Controller
    {
        public PreadmissionNoticeRegistrationReportInterface _con;

        public PreadmissionNoticeRegistrationReportFacade(PreadmissionNoticeRegistrationReportInterface conce)
        {
            _con = conce;
        }

        [HttpPost]
        [Route("get_intial_data")]
        public PreadmissionNoticeRegistrationReportDTO get_intial_data([FromBody] PreadmissionNoticeRegistrationReportDTO id)
        {           
            return _con.get_intial_data(id);
        }
        [Route("getprospectusno")]
        public PreadmissionNoticeRegistrationReportDTO getprospectusno([FromBody] PreadmissionNoticeRegistrationReportDTO id)
        {           
            return _con.getprospectusno(id);
        }
        [Route("getstudentlist")]
        public PreadmissionNoticeRegistrationReportDTO getstudentlist([FromBody] PreadmissionNoticeRegistrationReportDTO id)
        {           
            return _con.getstudentlist(id);
        }
        [Route("addtolist")]
        public PreadmissionNoticeRegistrationReportDTO addtolist([FromBody] PreadmissionNoticeRegistrationReportDTO id)
        {           
            return _con.addtolist(id);
        }
        [Route("Savedata")]
        public PreadmissionNoticeRegistrationReportDTO Savedata([FromBody] PreadmissionNoticeRegistrationReportDTO id)
        {           
            return _con.Savedata(id);
        }
        [Route("viewstudent")]
        public PreadmissionNoticeRegistrationReportDTO viewstudent([FromBody] PreadmissionNoticeRegistrationReportDTO id)
        {           
            return _con.viewstudent(id);
        }
        [Route("Editdata")]
        public PreadmissionNoticeRegistrationReportDTO Editdata([FromBody] PreadmissionNoticeRegistrationReportDTO id)
        {
            return _con.Editdata(id);
        }
        [Route("printData")]
        public PreadmissionNoticeRegistrationReportDTO printData([FromBody] PreadmissionNoticeRegistrationReportDTO id)
        {
            return _con.printData(id);
        }

    }
}
