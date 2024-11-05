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
    public class MasterLifeSkillAreaController : Controller
    {

        MasterLifeSkillAreaDelegates obj = new MasterLifeSkillAreaDelegates();

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
        public MasterLifeSkillAreaDTO Getdetails( MasterLifeSkillAreaDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.Getdetails(data);
        }
        // POST api/values
        [Route("savedata")]
        public MasterLifeSkillAreaDTO savedata([FromBody] MasterLifeSkillAreaDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.savedata(data);
        }

        [Route("editdetails/{id:int}")]
        public MasterLifeSkillAreaDTO editdetails(int ID)
        {
            return obj.editdetails(ID);
        }

        [Route("deactivate")]
        public MasterLifeSkillAreaDTO deactivate([FromBody] MasterLifeSkillAreaDTO data)
        {
            return obj.deactivate(data);
        }

        [Route("validateordernumber")]
        public MasterLifeSkillAreaDTO validateordernumber([FromBody] MasterLifeSkillAreaDTO data)
        {
            // data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.validateordernumber(data);
        }
    }
}
