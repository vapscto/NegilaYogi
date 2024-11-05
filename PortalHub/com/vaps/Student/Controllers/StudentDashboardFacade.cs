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
    public class StudentDashboardFacade : Controller
    {
        public StudentDashboardInterface _ads;

        public StudentDashboardFacade(StudentDashboardInterface adstu)
        {
            _ads = adstu;
        }

        // POST api/values

        [HttpPost]
        [Route("Getdetails")]
        public Task<StudentDashboardDTO> Getdetails([FromBody]StudentDashboardDTO sddto)
        {
            return _ads.Getdetails(sddto);
        }

        [Route("saveakpkfile")]
        public StudentDashboardDTO saveakpkfile([FromBody] StudentDashboardDTO data)
        {
            return _ads.saveakpkfile(data);
        }

        [Route("saverequest")]
        public StudentDashboardDTO saverequest([FromBody] StudentDashboardDTO data)
        {
            return _ads.saverequest(data);
        }

        [Route("getImages")]
        public Task<StudentDashboardDTO> getImages([FromBody] StudentDashboardDTO data)
        {
            return _ads.getImages(data);
        }

        [Route("viewData")]
        public StudentDashboardDTO viewData([FromBody] StudentDashboardDTO data)
        {
            return _ads.viewData(data);
        }

        [Route("conformdata")]
        public StudentDashboardDTO conformdata([FromBody] StudentDashboardDTO data)
        {
            return _ads.conformdata(data);
        }

        [Route("savecls_doc")]
        public StudentDashboardDTO savecls_doc([FromBody] StudentDashboardDTO data)
        {
            return _ads.savecls_doc(data);
        }
        [Route("savehome_doc")]
        public StudentDashboardDTO savehome_doc([FromBody] StudentDashboardDTO data)
        {
            return _ads.savehome_doc(data);
        }

        [Route("viewData_doc")]
        public StudentDashboardDTO viewData_doc([FromBody] StudentDashboardDTO data)
        {
            return _ads.viewData_doc(data);
        }

        [Route("viewnotice")]
        public StudentDashboardDTO viewnotice([FromBody] StudentDashboardDTO data)
        {
            return _ads.viewnotice(data);
        }

        [Route("onclick_notice")]
        public StudentDashboardDTO onclick_notice([FromBody] StudentDashboardDTO data)
        {
            return _ads.onclick_notice(data);
        }

        [Route("onclick_TT")]
        public StudentDashboardDTO onclick_TT([FromBody] StudentDashboardDTO data)
        {
            return _ads.onclick_TT(data);
        }

        [Route("onclick_syllabus")]
        public StudentDashboardDTO onclick_syllabus([FromBody] StudentDashboardDTO data)
        {
            return _ads.onclick_syllabus(data);
        }

        [Route("onclick_LIB")]
        public StudentDashboardDTO onclick_LIB([FromBody] StudentDashboardDTO data)
        {
            return _ads.onclick_LIB(data);
        }

        [Route("onclick_Homework")]
        public StudentDashboardDTO onclick_Homework([FromBody] StudentDashboardDTO data)
        {
            return _ads.onclick_Homework(data);
        }

        [Route("onclick_Classwork")]
        public StudentDashboardDTO onclick_Classwork([FromBody] StudentDashboardDTO data)
        {
            return _ads.onclick_Classwork(data);
        }

        [Route("onclick_Sports")]
        public StudentDashboardDTO onclick_Sports([FromBody] StudentDashboardDTO data)
        {
            return _ads.onclick_Sports(data);
        }

        [Route("onclick_Inventory")]
        public StudentDashboardDTO onclick_Inventory([FromBody] StudentDashboardDTO data)
        {
            return _ads.onclick_Inventory(data);
        }

        [Route("onclick_PDA")]
        public StudentDashboardDTO onclick_PDA([FromBody] StudentDashboardDTO data)
        {
            return _ads.onclick_PDA(data);
        }

        [Route("onclick_Gallery")]
        public StudentDashboardDTO onclick_Gallery([FromBody] StudentDashboardDTO data)
        {
            return _ads.onclick_Gallery(data);
        }

        [Route("onclick_Homework_load")]
        public StudentDashboardDTO onclick_Homework_load([FromBody] StudentDashboardDTO data)
        {
            return _ads.onclick_Homework_load(data);
        }

        [Route("onclick_Classwork_load")]
        public StudentDashboardDTO onclick_Classwork_load([FromBody] StudentDashboardDTO data)
        {
            return _ads.onclick_Classwork_load(data);
        }

        [Route("ViewStudentProfile")]
        public StudentDashboardDTO ViewStudentProfile([FromBody] StudentDashboardDTO data)
        {
            return _ads.ViewStudentProfile(data);
        }

        [Route("ViewMonthWiseAttendance")]
        public StudentDashboardDTO ViewMonthWiseAttendance([FromBody] StudentDashboardDTO data)
        {
            return _ads.ViewMonthWiseAttendance(data);
        }

        [Route("ViewYearWiseFee")]
        public StudentDashboardDTO ViewYearWiseFee([FromBody] StudentDashboardDTO data)
        {
            return _ads.ViewYearWiseFee(data);
        }

        [Route("ViewExamSubjectWiseDetails")]
        public StudentDashboardDTO ViewExamSubjectWiseDetails([FromBody] StudentDashboardDTO data)
        {
            return _ads.ViewExamSubjectWiseDetails(data);
        }

        [Route("onclick_Homework_seen")]
        public StudentDashboardDTO onclick_Homework_seen([FromBody] StudentDashboardDTO data)
        {
            return _ads.onclick_Homework_seen(data);
        }

        [Route("onclick_classwork_seen")]
        public StudentDashboardDTO onclick_classwork_seen([FromBody] StudentDashboardDTO data)
        {
            return _ads.onclick_classwork_seen(data);
        }

        [Route("onclick_noticeboard_seen")]
        public StudentDashboardDTO onclick_noticeboard_seen([FromBody] StudentDashboardDTO data)
        {
            return _ads.onclick_noticeboard_seen(data);
        }



        [Route("onclick_Homework_datewise")]
        public StudentDashboardDTO onclick_Homework_datewise([FromBody] StudentDashboardDTO data)
        {
            return _ads.onclick_Homework_datewise(data);
        }

        [Route("onclick_classwork_datewise")]
        public StudentDashboardDTO onclick_classwork_datewise([FromBody] StudentDashboardDTO data)
        {
            return _ads.onclick_classwork_datewise(data);
        }

        [Route("onclick_noticeboard_datewise")]
        public StudentDashboardDTO onclick_noticeboard_datewise([FromBody] StudentDashboardDTO data)
        {
            return _ads.onclick_noticeboard_datewise(data);
        }
        [Route("onclick_notice_datewise")]
        public StudentDashboardDTO onclick_notice_datewise([FromBody] StudentDashboardDTO data)
        {
            return _ads.onclick_notice_datewise(data);
        }

        [Route("onclick_Staff_details")]
        public StudentDashboardDTO onclick_Staff_details([FromBody] StudentDashboardDTO data)
        {
            return _ads.onclick_Staff_details(data);
        }
    }
}