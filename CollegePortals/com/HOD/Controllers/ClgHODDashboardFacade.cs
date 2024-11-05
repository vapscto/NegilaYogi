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
    public class ClgHODDashboardFacade :Controller
    {
        public ClgHODDashboardInterface _ads;

        public ClgHODDashboardFacade(ClgHODDashboardInterface adstu)
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

        [Route("save")]
        public ClgStudentDashboardDTO save([FromBody] ClgStudentDashboardDTO data)
        {
            return _ads.savedata(data);
        }

        [Route("mappHOD")]
        public ClgStudentDashboardDTO mappHOD([FromBody] ClgStudentDashboardDTO data)
        {
            return _ads.mappHODdata(data);
        }

        [Route("updateHOD")]
        public ClgStudentDashboardDTO updateHOD([FromBody] ClgStudentDashboardDTO data)
        {
            return _ads.updateHOD(data);
        }
        [Route("deactiveY")]
        public ClgStudentDashboardDTO deactiveY([FromBody] ClgStudentDashboardDTO data)
        {
            return _ads.deactiveY(data);
        }

    }
}
