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
    public class StudentFeedbackFormFacade : Controller
    {
        public StudentFeedbackFormInterface _ads;

        public StudentFeedbackFormFacade(StudentFeedbackFormInterface adstu)
        {
            _ads = adstu;
        }

        [HttpPost]
        [Route("getloaddata")]      
        public StudentFeedbackFormDTO getloaddata([FromBody]StudentFeedbackFormDTO data)
        {
            return _ads.getloaddata(data);
        }

        [HttpPost]
        [Route("savefeedback")]
        public StudentFeedbackFormDTO savefeedback([FromBody]StudentFeedbackFormDTO data)
        {
            return _ads.savefeedback(data);
        }
        [HttpPost]
        [Route("deactive")]
        public StudentFeedbackFormDTO deactive([FromBody]StudentFeedbackFormDTO data)
        {
            return _ads.deactive(data);
        }
        
    }
}
