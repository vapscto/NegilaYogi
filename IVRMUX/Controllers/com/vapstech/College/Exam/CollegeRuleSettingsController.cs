using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Exam
{
    [Route("api/[controller]")]
    public class CollegeRuleSettingsController : Controller
    {
        public CollegeRuleSettingsDelegate _delg = new CollegeRuleSettingsDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [Route("getalldetails/{id:int}")]
        public CollegeRuleSettingsDTO getalldetails (int id)
        {
            CollegeRuleSettingsDTO data = new CollegeRuleSettingsDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getalldetails(data);
        }
        [Route("getbranch")]
        public CollegeRuleSettingsDTO getbranch([FromBody]  CollegeRuleSettingsDTO data)
        {           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getbranch(data);
        }
        [Route("get_semesters")]
        public CollegeRuleSettingsDTO get_semesters([FromBody]  CollegeRuleSettingsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_semesters(data);
        }

        [Route("get_subjectscheme")]
        public CollegeRuleSettingsDTO get_subjectscheme([FromBody]  CollegeRuleSettingsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_subjectscheme(data);
        }
        [Route("get_schemetype")]
        public CollegeRuleSettingsDTO get_schemetype([FromBody]  CollegeRuleSettingsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_schemetype(data);
        }
        [Route("get_subjects")]
        public CollegeRuleSettingsDTO get_subjects([FromBody]  CollegeRuleSettingsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_subjects(data);
        }
        [Route("saveddata")]
        public CollegeRuleSettingsDTO saveddata([FromBody]  CollegeRuleSettingsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.saveddata(data);
        }
        [Route("getalldetailsviewrecords")]
        public CollegeRuleSettingsDTO getalldetailsviewrecords([FromBody]  CollegeRuleSettingsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getalldetailsviewrecords(data);
        }
        [Route("viewrecordspopup_subgrps")]
        public CollegeRuleSettingsDTO viewrecordspopup_subgrps([FromBody]  CollegeRuleSettingsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.viewrecordspopup_subgrps(data);
        }
        [Route("getalldetailsviewrecords_sub_grp_exms")]
        public CollegeRuleSettingsDTO getalldetailsviewrecords_sub_grp_exms([FromBody]  CollegeRuleSettingsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getalldetailsviewrecords_sub_grp_exms(data);
        }
        [Route("editdeatils")]
        public CollegeRuleSettingsDTO editdeatils([FromBody]  CollegeRuleSettingsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.editdeatils(data);
        }

        [Route("deactivate")]
        public CollegeRuleSettingsDTO deactivate([FromBody]  CollegeRuleSettingsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.deactivate(data);
        }
        [Route("deactivatesubject")]
        public CollegeRuleSettingsDTO deactivatesubject([FromBody]  CollegeRuleSettingsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.deactivatesubject(data);
        }
        [Route("deactivategroup")]
        public CollegeRuleSettingsDTO deactivategroup([FromBody]  CollegeRuleSettingsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.deactivategroup(data);
        }
        [Route("deactivateexam")]
        public CollegeRuleSettingsDTO deactivateexam([FromBody]  CollegeRuleSettingsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.deactivateexam(data);
        }





    }
}
