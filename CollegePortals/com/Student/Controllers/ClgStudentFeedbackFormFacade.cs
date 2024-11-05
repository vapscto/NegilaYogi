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
    public class ClgStudentFeedbackFormFacade : Controller
    {
        public ClgStudentFeedbackFormInterface _ads;

        public ClgStudentFeedbackFormFacade(ClgStudentFeedbackFormInterface adstu)
        {
            _ads = adstu;
        }

        [HttpPost]
        [Route("getloaddata")]
        public ClgStudentFeedbackFormDTO getloaddata([FromBody]ClgStudentFeedbackFormDTO data)
        {
            return _ads.getloaddata(data);
        }
        [HttpPost]
        [Route("savefeedback")]
        public ClgStudentFeedbackFormDTO savefeedback([FromBody]ClgStudentFeedbackFormDTO data)
        {
            return _ads.savefeedback(data);
        }
        [HttpPost]
        [Route("deactive")]
        public ClgStudentFeedbackFormDTO deactive([FromBody]ClgStudentFeedbackFormDTO data)
        {
            return _ads.deactive(data);
        }
        


    }
}
