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
    public class ClgCOEFacade : Controller
    {
        public ClgCOEInterface _ads;

        public ClgCOEFacade(ClgCOEInterface adstu)
        {
            _ads = adstu;
        }

        [HttpPost]
        [Route("getloaddata")]
        public ClgStudentDashboardDTO getloaddata([FromBody]ClgStudentDashboardDTO data)
        {
            return _ads.getloaddata(data);
        }
        [HttpPost]
        [Route("getcoedata")]
        public ClgStudentDashboardDTO getcoedata([FromBody]ClgStudentDashboardDTO data)
        {
            return _ads.getcoedata(data);
        }

    }
}
