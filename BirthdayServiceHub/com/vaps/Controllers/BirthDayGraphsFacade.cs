using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirthdayServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.BirthDay;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BirthdayServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class BirthDayGraphsFacade : Controller
    {
        public BirthDayGraphsInterface _inter;
        public BirthDayGraphsFacade(BirthDayGraphsInterface brth)
        {
            _inter = brth;
        }

        [HttpGet("{id:int}")]
        //[Route("getdata/{id:int}")]
        public BirthDayDTO getdata(int id)
        {
            return _inter.getdata(id);
        }

        [HttpPost]
        [Route("getS")]
        public BirthDayDTO getclasssectionstudentlist([FromBody]BirthDayDTO student)
        {
            return _inter.getlistthree(student);
        }
        [Route("BindStaff")]
        public BirthDayDTO getstafflist([FromBody]BirthDayDTO staff)
        {
            return _inter.staflist(staff);
        }
        [Route("Sendsms")]
        public BirthDayDTO getstafflist1([FromBody]BirthDayDTO staff)
        {
            return _inter.staflist1(staff);
        }
        [Route("Check_SMS_Mail_Status/{id:int}")]

      
        [Route("Sendmsg")]
        public BirthDayDTO Sendmsg([FromBody] BirthDayDTO msg)
        {
            return _inter.Sendmsg(msg);
        }
        [Route("getReport")]
        public BirthDayDTO getReport([FromBody] BirthDayDTO data)
        {
            return _inter.getReport(data);
        }
        [Route("getEmailSMSCount")]
        public BirthDayDTO getEmailSMSCount([FromBody] BirthDayDTO data)
        {
            return _inter.getEmailSMSCount(data);
        }
        [Route("SearchByColumn")]
        public BirthDayDTO SearchByColumn([FromBody] BirthDayDTO obj)
        {
            return _inter.SearchByColumn(obj);
        }

        [Route("getmonthreport")]
        public BirthDayDTO getmonthreport([FromBody] BirthDayDTO rpt)
        {
            return _inter.getmonthreport(rpt);
        }

        [Route("getstaffdetails")]
        public Task<BirthDayDTO> getstaffdetails([FromBody] BirthDayDTO obj)
        {
            return _inter.getstaffdetails(obj);
        }

     
    }
}
