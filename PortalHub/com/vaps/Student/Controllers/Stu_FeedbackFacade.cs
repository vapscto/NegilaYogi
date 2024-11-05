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
    public class Stu_FeedbackFacade : Controller
    {
        public Stu_FeedbackInterface _ads;

        public Stu_FeedbackFacade(Stu_FeedbackInterface adstu)
        {
            _ads = adstu;
        }

        [HttpPost]
        [Route("getloaddata")]      
        public Stu_FeedbackDTO getloaddata([FromBody]Stu_FeedbackDTO data)
        {
            return _ads.getloaddata(data);
        }

        [HttpPost]
        [Route("savecomment")]
        public Stu_FeedbackDTO savecomment([FromBody]Stu_FeedbackDTO sddto)
        {
            return _ads.savecomment(sddto);
        }
        [HttpPost]
        [Route("getexamdetails")]
        public Stu_FeedbackDTO getexamdetails([FromBody]Stu_FeedbackDTO sddto)
        {
            return _ads.getexamdetails(sddto);
        }
    }
}
