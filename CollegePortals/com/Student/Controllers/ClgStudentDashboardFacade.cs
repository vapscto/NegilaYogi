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
using CollegePortals.com.Student.Interfaces;
using PreadmissionDTOs.com.vaps.College.Portals;

namespace CollegePortals.com.Student.Controllers
{
    [Route("api/[controller]")]
    public class ClgStudentDashboardFacade :Controller
    {
        public ClgStudentDashboardInterface _ads;

        public ClgStudentDashboardFacade(ClgStudentDashboardInterface adstu)
        {
            _ads = adstu;
        }

        // POST api/values

        [HttpPost]
        [Route("Getdetails")]       
        public Task<ClgStudentDashboardDTO> Getdetails([FromBody]ClgStudentDashboardDTO data)
        {
            return _ads.Getdetails(data);
        }

        [Route("ViewStudentProfile")]
        public ClgStudentDashboardDTO ViewStudentProfile([FromBody] ClgStudentDashboardDTO data)
        {
            return _ads.ViewStudentProfile(data);
        }
        [Route("onclick_syllabus")]
        public ClgStudentDashboardDTO onclick_syllabus([FromBody] ClgStudentDashboardDTO data)
        {
            return _ads.onclick_syllabus(data);
        }

        [Route("onclick_notice")]
        public ClgStudentDashboardDTO onclick_notice([FromBody] ClgStudentDashboardDTO data)
        {
            return _ads.onclick_notice(data);
        }
        [Route("onclick_noticeboard_seen")]
        public ClgStudentDashboardDTO onclick_noticeboard_seen([FromBody] ClgStudentDashboardDTO data)
        {
            return _ads.onclick_noticeboard_seen(data);
        }

        [Route("ViewMonthWiseAttendance")]
        public ClgStudentDashboardDTO ViewMonthWiseAttendance([FromBody] ClgStudentDashboardDTO data)
        {
            return _ads.ViewMonthWiseAttendance(data);
        }

    }
}
