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
    public class ExamReportFacade : Controller
    {
        public ExamReportInterface _ads;

        public ExamReportFacade(ExamReportInterface adstu)
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
        [Route("getexamdata")]
        public Task<ExamDTO> getexamdata([FromBody]ExamDTO sddto)
        {
            return _ads.getexamdata(sddto);
        }
        [HttpPost]
        [Route("getSubjects")]
        public ExamDTO getSubjects([FromBody]ExamDTO sddto)
        {
            return _ads.getSubjects(sddto);
        }

        [HttpPost]
        [Route("StudentExamDetails")]
        public Task<ExamDTO> StudentExamDetails([FromBody]ExamDTO dto)
        {
            return _ads.StudentExamDetails(dto);
        }
    }
}
