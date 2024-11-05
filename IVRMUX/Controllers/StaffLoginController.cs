using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Hosting;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class StaffLoginController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        StaffLoginDelegate stafflog = new StaffLoginDelegate();
        public StaffLoginController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
       

        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public StaffLoginDTO Get(int id)
        {

            StaffLoginDTO en = new StaffLoginDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            en.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            en.Id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            en.ASMAY_Id = ASMAY_Id;

            int roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            en.roleId = roleidd;

            return stafflog.getmoduledet(en);
        }

        [Route("getmodulerolesinswise/{id:int}")]
        public StaffLoginDTO getmodroleins(int id)
        {
            return stafflog.getmoduleroledetails(id);
        }

        [HttpPost]
        [Route("getpagedetailsrolemodulewise")]
        public StaffLoginDTO getpagename([FromBody] StaffLoginDTO pgmodu)
        {
            return stafflog.getpagedetails(pgmodu);
        }

        [Route("updateuser")]
        public StaffLoginDTO updateuser([FromBody] StaffLoginDTO pgmodu)
        {
            return stafflog.updateusername(pgmodu);
        }


        [Route("searchfilter")]
        public StaffLoginDTO searchfilter([FromBody]StaffLoginDTO student)
        {
           // student.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return stafflog.searchfilter(student);
        }

        [Route("getstudata")]
        public StaffLoginDTO getstudata([FromBody]StaffLoginDTO sddto)
        {
            //sddto.User_Name = Convert.ToString(HttpContext.Session.GetString("UserName"));

            return stafflog.getstudata(sddto);
        }

        [Route("onchangeuser")]
        public StaffLoginDTO onchangeuser([FromBody]StaffLoginDTO sddto)
        {
            //sddto.User_Name = Convert.ToString(HttpContext.Session.GetString("UserName"));

            return stafflog.onchangeuser(sddto);
        }

        [Route("multionchangeuser")]
        public StaffLoginDTO multionchangeuser([FromBody]StaffLoginDTO sddto)
        {
            //sddto.User_Name = Convert.ToString(HttpContext.Session.GetString("UserName"));

            return stafflog.multionchangeuser(sddto);
        }

        [Route("multiuserdeletpages")]
        public StaffLoginDTO multiuserdeletpages([FromBody]StaffLoginDTO sddto)
        {
            //sddto.User_Name = Convert.ToString(HttpContext.Session.GetString("UserName"));

            return stafflog.multiuserdeletpages(sddto);
        }

        // POST api/values
        [HttpPost]
        public StaffLoginDTO savedata([FromBody] StaffLoginDTO pgmodu)
        {
            //int trustid = 0;
            //if (HttpContext.Session.GetString("pagemoduleid") != null)
            //{
            //    trustid = Convert.ToInt32(HttpContext.Session.GetString("pagemoduleid"));//Get
            //}

            //pgmodu.IVRMMP_Id = trustid;
            //HttpContext.Session.Remove("pagemoduleid");

            int roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            pgmodu.roleId = roleidd;

            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            pgmodu.Machine_Ip_Address = remoteIpAddress.ToString();

            return stafflog.savedetails(pgmodu);
        }

        [HttpPost("{id}")]
        public StaffLoginDTO Put(int id, [FromBody] StaffLoginDTO value)
        {
            return stafflog.getfilterde(id, value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

     

        [HttpPost]
        [Route("checkdupli")]
        public StaffLoginDTO checkduplicateusername([FromBody] StaffLoginDTO pgmodu)
        {
            return stafflog.checkusernmedup(pgmodu);
        }

        [Route("deletemodpages")]
        public StaffLoginDTO Delete([FromBody] StaffLoginDTO id)
        {
            return stafflog.deleterec(id);
        }


        [Route("changeinsti")]
        public StaffLoginDTO changeinstitu([FromBody] StaffLoginDTO data)
        {
            return stafflog.changeinstitu(data);
        }


        [HttpPost]
        [Route("checktrust")]
        public StaffLoginDTO checktrustfun([FromBody] StaffLoginDTO pgmodu)
        {
            return stafflog.checktrustfun(pgmodu);
        }


        [HttpPost]
        [Route("getstaffmobilepages")]
        public StaffLoginDTO getstaffmobilepages([FromBody] StaffLoginDTO pgmodu)
        {
           

            //int roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            //pgmodu.roleId = roleidd;
            return stafflog.getstaffmobilepages(pgmodu);
        }

    }
}
