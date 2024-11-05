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
using PreadmissionDTOs.com.vaps.Portals.Student;
using CollegePortals.com.Student.Interfaces;
using PreadmissionDTOs.com.vaps.College.Portals.Student;

namespace CollegePortals.com.Student.Controllers
{
    [Route("api/[controller]")]
    public class ClgExamReportFacade : Controller
    {
        public ClgExamReportInterface _ads;

        public ClgExamReportFacade(ClgExamReportInterface adstu)
        {
            _ads = adstu;
        }

        [HttpPost]
        [Route("getloaddata")]      
        public ClgExamDTO getloaddata([FromBody]ClgExamDTO data)
        {
            return _ads.getloaddata(data);
        }

        [HttpPost]
        [Route("getexamdata")]
        public Task<ClgExamDTO> getexamdata([FromBody]ClgExamDTO sddto)
        {
            return _ads.getexamdata(sddto);
        }
        [HttpPost]
        [Route("getSubjects")]
        public ClgExamDTO getSubjects([FromBody]ClgExamDTO sddto)
        {
            return _ads.getSubjects(sddto);
        }

        [HttpPost]
        [Route("StudentExamDetails")]
        public Task<ClgExamDTO> StudentExamDetails([FromBody]ClgExamDTO dto)
        {
            return _ads.StudentExamDetails(dto);
        }
    }
}
