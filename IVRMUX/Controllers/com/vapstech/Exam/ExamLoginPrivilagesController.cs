
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [Route("api/[controller]")]
    public class ExamLoginPrivilagesController : Controller
    {


        ExamLoginPrivilagesDelegates exammasterdelStr = new ExamLoginPrivilagesDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public Exm_Login_PrivilegeDTO Getdetails(Exm_Login_PrivilegeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterdelStr.Getdetails(data);            
        }
        [HttpPost]
        [Route("editdetails")]
        public Exm_Login_PrivilegeDTO editdetails([FromBody] Exm_Login_PrivilegeDTO data)
        {
            return exammasterdelStr.editdetails(data);
        }


        [HttpPost]
        [Route("getalldetailsviewrecords")]
        public Exm_Login_PrivilegeDTO getalldetailsviewrecords([FromBody] Exm_Login_PrivilegeDTO data)
        {
            return exammasterdelStr.getalldetailsviewrecords(data);
        }
        

        [Route("getclstechdetails")]
        public Exm_Login_PrivilegeDTO getclstechdetails([FromBody] Exm_Login_PrivilegeDTO data)
        {
            //Exm_Login_PrivilegeDTO data = new Exm_Login_PrivilegeDTO();
            //data.HRME_Id = Convert.ToInt64(ID);
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterdelStr.getclstechdetails(data);
        }

        [Route("savedetails")]
        public Exm_Login_PrivilegeDTO savedetails([FromBody] Exm_Login_PrivilegeDTO data)
        {     
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterdelStr.savedetails(data);
        }

        [Route("deactivate")]
        public Exm_Login_PrivilegeDTO deactivate([FromBody] Exm_Login_PrivilegeDTO data)
        {
            return exammasterdelStr.deactivate(data);         
        }
        [Route("OnAcdyear")]
        public Exm_Login_PrivilegeDTO OnAcdyear([FromBody] Exm_Login_PrivilegeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterdelStr.OnAcdyear(data);
        }
        
    }

}
