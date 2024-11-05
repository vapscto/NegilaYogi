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
using PreadmissionDTOs.com.vaps.Portals.IVRM;

namespace PortalHub.com.vaps.Student.Controllers
{
    [Route("api/[controller]")]
    public class IVRM_InteractionsFacade : Controller
    {
        public IVRM_InteractionsInterface _ads;

        public IVRM_InteractionsFacade(IVRM_InteractionsInterface adstu)
        {
            _ads = adstu;
        }

        [HttpPost]
        [Route("getloaddata")]
        public Task<IVRM_School_InteractionsDTO> getloaddata([FromBody]IVRM_School_InteractionsDTO data)
        {
            return _ads.getloaddata(data);
        }
        [Route("getdetails")]
        public Task<IVRM_School_InteractionsDTO> getdetails([FromBody]IVRM_School_InteractionsDTO data)
        {
            return _ads.getdetails(data);
        }


        [Route("getstudent")]
        public Task<IVRM_School_InteractionsDTO> getstudent([FromBody]IVRM_School_InteractionsDTO data)
        {
            return _ads.getstudent(data);
        }

        [Route("savedetails")]
        public IVRM_School_InteractionsDTO savedetails([FromBody]IVRM_School_InteractionsDTO data)
        {
            return _ads.savedetails(data);
        }

        [Route("savereply")]
        public IVRM_School_InteractionsDTO savereply([FromBody]IVRM_School_InteractionsDTO data)
        {
            return _ads.savereply(data);
        }
        [Route("deletemsg")]
        public IVRM_School_InteractionsDTO deletemsg([FromBody]IVRM_School_InteractionsDTO data)
        {
            return _ads.deletemsg(data);
        }

        [Route("deleteinboxmsg")]
        public IVRM_School_InteractionsDTO deleteinboxmsg([FromBody]IVRM_School_InteractionsDTO data)
        {
            return _ads.deleteinboxmsg(data);
        }
        [Route("reply")]
        public Task<IVRM_School_InteractionsDTO> reply([FromBody]IVRM_School_InteractionsDTO data)
        {
            return _ads.reply(data);
        }

        [Route("seen")]
        public IVRM_School_InteractionsDTO seen([FromBody]IVRM_School_InteractionsDTO data)
        {
            return _ads.seen(data);
        }


    }
}
