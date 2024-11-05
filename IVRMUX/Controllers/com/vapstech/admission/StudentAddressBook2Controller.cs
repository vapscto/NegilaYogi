using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.admission;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using corewebapi18072016.Delegates.com.vapstech.admission;
using CommonLibrary;


namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class StudentAddressBook2Controller : Controller
    {

        StudentAddressBook2Delegate st = new StudentAddressBook2Delegate();

        //// GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getinitialdata/{id:int}")]
        public StudentAddressBook2DTO getInitialData(int id)
        {
             id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));           
            return st.getData(id);
        }        

        [Route("classchange")]
        public StudentAddressBook2DTO classchange()
        {
            int id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));           
            return st.getData1(id);
        }

        [Route("yearchange")]
        public StudentAddressBook2DTO yearchange(StudentAddressBook2DTO data)
        {
            data.MI_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return st.yearchange(data);
        }

        [Route("sectionchange")]
        public StudentAddressBook2DTO sectionchange([FromBody]StudentAddressBook2DTO get_St)
        {
            get_St.MI_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return st.sectionchange(get_St);
        }
        

        [HttpPost]
        [Route("getdetails")]
        public StudentAddressBook2DTO getdetails([FromBody]StudentAddressBook2DTO MMD)
        {
             MMD.MI_id  = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return st.getdetails(MMD);
        }

        [Route("getdetailsstdemp")]
        public StudentAddressBook2DTO getdetailsstdemp([FromBody]StudentAddressBook2DTO MMD)
        {
            MMD.MI_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return st.getdetailsstdemp(MMD);
        }        

        [Route("ExportToExcle/")]
        public string ExportToExcle([FromBody] StudentAddressBook2DTO MMD)
        {
            return st.ExportToExcle(MMD);
        }

        [Route("yearchangenew")]
        public StudentAddressBook2DTO yearchangenew([FromBody]StudentAddressBook2DTO MMD)
        {
            MMD.MI_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return st.yearchangenew(MMD);
        }
        [Route("classchangenew")]
        public StudentAddressBook2DTO classchangenew([FromBody]StudentAddressBook2DTO MMD)
        {
            MMD.MI_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return st.classchangenew(MMD);
        }
        [Route("sectionchangenew")]
        public StudentAddressBook2DTO sectionchangenew([FromBody]StudentAddressBook2DTO MMD)
        {
            MMD.MI_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return st.sectionchangenew(MMD);
        }
        [Route("getdetailsnew")]
        public StudentAddressBook2DTO getdetailsnew([FromBody]StudentAddressBook2DTO MMD)
        {
            MMD.MI_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return st.getdetailsnew(MMD);
        }

    }
}
