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
using PreadmissionDTOs.com.vaps.College.Student;

namespace CollegePortals.com.vaps.Student.Controllers
{
    [Route("api/[controller]")]
    public class CollegeStudent_TTFacade : Controller
    {
        public CollegeStudent_TTInterface _ads;

        public CollegeStudent_TTFacade(CollegeStudent_TTInterface adstu)
        {
            _ads = adstu;
        }


  
        [Route("getloaddata")]      
        public CollegeStudent_TTDTO getloaddata([FromBody]CollegeStudent_TTDTO data)
        {
            return _ads.getloaddata(data);
        }

        [HttpPost]
        [Route("getStudentTT")]
        public Task<CollegeStudent_TTDTO> getStudentTT([FromBody]CollegeStudent_TTDTO sddto)
        {
            return _ads.getStudentTT(sddto);
        }
    }
}
