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
using PreadmissionDTOs.com.vaps.Fees;

namespace PortalHub.com.vaps.Student.Controllers
{
    [Route("api/[controller]")]
    public class FeeReceiptFacade : Controller
    {
        public FeeReceiptInterface _ads;

        public FeeReceiptFacade(FeeReceiptInterface adstu)
        {
            _ads = adstu;
        }


        [HttpPost]
        [Route("getloaddata")]      
        public StudentDashboardDTO getloaddata([FromBody]StudentDashboardDTO data)
        {
            return _ads.getloaddata(data);
        }

        [HttpPost]
        [Route("printreceipt")]
        public Task<FeeStudentTransactionDTO> printreceipt([FromBody]FeeStudentTransactionDTO sddto)
        {
            return _ads.printreceipt(sddto);
        }
        [HttpPost]
        [Route("getrecdetails")]
        public StudentDashboardDTO getrecdetails([FromBody]StudentDashboardDTO sddto)
        {
            return _ads.getrecdetails(sddto);
        }

        [HttpPost]
        [Route("getstudetails")]
        public StudentDashboardDTO getstudetails([FromBody]StudentDashboardDTO sddto)
        {
            return _ads.getstudetails(sddto);
        }

        [HttpPost]
        [Route("preadmissiongetrecdetails")]
        public StudentDashboardDTO preadmissiongetrecdetails([FromBody]StudentDashboardDTO sddto)
        {
            return _ads.preadmissiongetrecdetails(sddto);
        }
        [HttpPost]
        [Route("preadmissiongetdetails")]
        public StudentDashboardDTO preadmissiongetdetails([FromBody]StudentDashboardDTO sddto)
        {
            return _ads.preadmissiongetdetails(sddto);
        }
        

    }
}
