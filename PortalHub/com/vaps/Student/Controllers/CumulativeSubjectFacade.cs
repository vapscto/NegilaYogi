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
    public class CumulativeSubjectFacade : Controller
    {
        public CumulativeSubjectInterface _ads;

        public CumulativeSubjectFacade(CumulativeSubjectInterface adstu)
        {
            _ads = adstu;
        }

        [HttpPost]
        [Route("getloaddata")]      
        public ExamDTO getloaddata([FromBody]ExamDTO data)
        {
            return _ads.getloaddata(data);
        }

        [HttpPost]
        [Route("getSubjectsdata")]
        public ExamDTO getSubjectsdata([FromBody]ExamDTO sddto)
        {
            return _ads.getSubjectsdata(sddto);
        }
        [HttpPost]
        [Route("getexamdetails")]
        public ExamDTO getexamdetails([FromBody]ExamDTO sddto)
        {
            return _ads.getexamdetails(sddto);
        }
    }
}
