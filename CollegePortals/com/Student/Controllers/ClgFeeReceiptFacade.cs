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
using CollegePortals.com.vaps.Student.Interfaces;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.College.Portals;
using PreadmissionDTOs.com.vaps.College.Fees;

namespace CollegePortals.com.vaps.Student.Controllers
{
    [Route("api/[controller]")]
    public class ClgFeeReceiptFacade : Controller
    {
        public ClgFeeReceiptInterface _ads;

        public ClgFeeReceiptFacade(ClgFeeReceiptInterface adstu)
        {
            _ads = adstu;
        }


        [HttpPost]
        [Route("getloaddata")]      
        public ClgPortalFeeDTO getloaddata([FromBody]ClgPortalFeeDTO data)
        {
            return _ads.getloaddata(data);
        }

        [HttpPost]
        [Route("printreceipt")]
        public Task<CollegeFeeTransactionDTO> printreceipt([FromBody]CollegeFeeTransactionDTO sddto)
        {
            return _ads.printreceipt(sddto);
        }
        [HttpPost]
        [Route("getrecdetails")]
        public ClgPortalFeeDTO getrecdetails([FromBody]ClgPortalFeeDTO sddto)
        {
            return _ads.getrecdetails(sddto);
        }
    }
}
