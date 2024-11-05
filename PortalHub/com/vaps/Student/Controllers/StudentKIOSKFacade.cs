using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using DomainModel.Model;
using DataAccessMsSqlServerProvider;
using MailKit.Net.Smtp;
using MimeKit;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using PortalHub.com.vaps.Student.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace PortalHub.com.vaps.Student.Controllers
{
    [Route("api/[controller]")]
    public class StudentKIOSKFacade : Controller
    {
        public StudentKIOSKInterface _ads;
        public StudentKIOSKFacade(StudentKIOSKInterface adstu)
        {
            _ads = adstu;
        }

        [HttpPost]
        [Route("GetdetailsKiosk")]
        public Task<StudentKIOSKDTO> GetdetailsKiosk([FromBody]StudentKIOSKDTO sddto)
        {
            return _ads.GetdetailsKiosk(sddto);
        }

        [Route("GetAttendancedetailsKiosk")]
        public Task<StudentKIOSKDTO> GetAttendancedetailsKiosk([FromBody] StudentKIOSKDTO data)
        {
            return _ads.GetAttendancedetailsKiosk(data);
        }

        [Route("showExamreport")]
        public StudentKioskExamTopperDTO showExamreport([FromBody] StudentKioskExamTopperDTO data)
        {
            return _ads.showExamreport(data);
        }

        [Route("getcoedata")]
        public StudentKioskCOEDTO getcoedata([FromBody] StudentKioskCOEDTO data)
        {
            return _ads.getcoedata(data);
        }

        [Route("getSubjectsdata")]
        public StudentKioskSubjectDTO getSubjectsdata([FromBody] StudentKioskSubjectDTO data)
        {
            return _ads.getSubjectsdata(data);
        }

        [Route("StudentExamDetails")]
        public Task<StudentKioskEXAMDTO> StudentExamDetails([FromBody] StudentKioskEXAMDTO data)
        {
            return _ads.StudentExamDetails(data);
        }

        [Route("getexamdata")]
        public Task<StudentKioskEXAMDTO> getexamdata([FromBody] StudentKioskEXAMDTO data)
        {
            return _ads.getexamdata(data);
        }

        [Route("getloaddataFEE")]
        public Task<StudentKioskFEEDTO> getloaddataFEE([FromBody] StudentKioskFEEDTO data)
        {
            return _ads.getloaddataFEE(data);
        }

        [Route("kioskSportsWinners")]
        public StudentKioskSPORTSDTO kioskSportsWinners([FromBody] StudentKioskSPORTSDTO data)
        {
            return _ads.kioskSportsWinners(data);
        }
        [Route("getstudentBD")]
        public StudentKioskBIRTHDAYDTO getstudentBD([FromBody] StudentKioskBIRTHDAYDTO data)
        {
            return _ads.getstudentBD(data);
        }
        [Route("getstaffdetails")]
        public Task<StudentKioskBIRTHDAYDTO> getstaffdetails([FromBody] StudentKioskBIRTHDAYDTO data)
        {
            return _ads.getstaffdetails(data);
        }

        [Route("GetHomeWorkdetailsKiosk")]
        public Task<StudentKioskHomeWorkDTO> GetHomeWorkdetailsKiosk([FromBody] StudentKioskHomeWorkDTO data)
        {
            return _ads.GetHomeWorkdetailsKiosk(data);
        }

        [Route("GetNoticedetailsKiosk")]
        public Task<StudentKioskNoticeDTO> GetNoticedetailsKiosk([FromBody] StudentKioskNoticeDTO data)
        {
            return _ads.GetNoticedetailsKiosk(data);
        }

        [Route("getStudentTT")]
        public Task<StudentKioskTimeTableDTO> getStudentTT([FromBody] StudentKioskTimeTableDTO data)
        {
            return _ads.getStudentTT(data);
        }

        [Route("getloadyear")]
        public StudentKioskSubjectDTO getloadyear([FromBody] StudentKioskSubjectDTO data)
        {
            return _ads.getloadyear(data);
        }
    }
}
