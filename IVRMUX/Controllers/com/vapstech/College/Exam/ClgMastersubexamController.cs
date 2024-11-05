using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Exam
{
    [Route("api/[controller]")]
    public class ClgMastersubexamController : Controller
    {
        ClgMastersubexamDelegates mastersubexamdelStr = new ClgMastersubexamDelegates();
        // GET: api/<controller>
        [HttpGet]
        [Route("Getdetails")]
        public mastersubexamDTO Getdetails(mastersubexamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastersubexamdelStr.Getdetails(data);
        }
        [HttpGet]
        [Route("editdeatils/{id:int}")]
        public mastersubexamDTO editdeatils(int ID)
        {
            return mastersubexamdelStr.editdeatils(ID);
        }

        [HttpPost]
        [Route("savedetails")]
        public mastersubexamDTO savedetails([FromBody] mastersubexamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastersubexamdelStr.savedetails(data);
        }
        [HttpPost]
        [Route("validateordernumber")]
        public mastersubexamDTO validateordernumber([FromBody] mastersubexamDTO data)
        {
            return mastersubexamdelStr.validateordernumber(data);
        }
        [HttpPost]
        [Route("deactivate")]
        public mastersubexamDTO deactivate([FromBody] mastersubexamDTO data)
        {
            return mastersubexamdelStr.deactivate(data);
        }
    }
}
