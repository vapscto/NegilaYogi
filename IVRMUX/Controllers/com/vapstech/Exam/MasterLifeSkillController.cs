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

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class MasterLifeSkillController : Controller
    {

        MasterLifeSkillDelegates obj = new MasterLifeSkillDelegates();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Getdetails(int id)
        {
            return "value";
        }

        [Route("Getdetails")]
        public MasterLifeSkillDTO Getdetails(MasterLifeSkillDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.Getdetails(data);
        }
        // POST api/values
        [Route("savedata")]
        public MasterLifeSkillDTO savedata([FromBody] MasterLifeSkillDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.savedata(data);
        }

        [Route("editdetails")]
        public MasterLifeSkillDTO editdetails([FromBody] MasterLifeSkillDTO data)
        {           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.editdetails(data);
        }

        [Route("deactivate")]
        public MasterLifeSkillDTO deactivate([FromBody] MasterLifeSkillDTO data)
        {
            return obj.deactivate(data);
        }

        //Master Life Skill Area

        [Route("Savedataarea")]
        public MasterLifeSkillDTO Savedataarea([FromBody] MasterLifeSkillDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.Savedataarea(data);
        }
        [Route("editdetailsarea")]
        public MasterLifeSkillDTO editdetailsarea([FromBody] MasterLifeSkillDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.editdetailsarea(data);
        }

        [Route("deactivatearea")]
        public MasterLifeSkillDTO deactivatearea([FromBody] MasterLifeSkillDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.deactivatearea(data);
        }

        [Route("validateordernumber")]
        public MasterLifeSkillDTO validateordernumber([FromBody] MasterLifeSkillDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.validateordernumber(data);
        }

        //Master Life Skill Area Mapping
        [Route("Savedataareamapping")]
        public MasterLifeSkillDTO Savedataareamapping([FromBody] MasterLifeSkillDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.Savedataareamapping(data);
        }
        [Route("editdetailsareamapping")]
        public MasterLifeSkillDTO editdetailsareamapping([FromBody] MasterLifeSkillDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.editdetailsareamapping(data);
        }

        [Route("deactivateareamapping")]
        public MasterLifeSkillDTO deactivateareamapping([FromBody] MasterLifeSkillDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.deactivateareamapping(data);
        }

        [Route("getgrade")]
        public MasterLifeSkillDTO getgrade([FromBody] MasterLifeSkillDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.getgrade(data);
        }
        

    }
}
