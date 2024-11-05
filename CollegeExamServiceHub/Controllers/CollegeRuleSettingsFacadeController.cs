using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeExamServiceHub.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeExamServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class CollegeRuleSettingsFacadeController : Controller
    {

        public CollegeRuleSettingsInterface _intf;

        public CollegeRuleSettingsFacadeController(CollegeRuleSettingsInterface _intfe)
        {
            _intf = _intfe;
        }

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

        [Route("getalldetails")]
        public CollegeRuleSettingsDTO getalldetails([FromBody] CollegeRuleSettingsDTO data)
        {
            return _intf.getalldetails(data);
        }
        [Route("getbranch")]
        public CollegeRuleSettingsDTO getbranch([FromBody]  CollegeRuleSettingsDTO data)
        {
            return _intf.getbranch(data);
        }
        [Route("get_semesters")]
        public CollegeRuleSettingsDTO get_semesters([FromBody]  CollegeRuleSettingsDTO data)
        {
            return _intf.get_semesters(data);
        }

        [Route("get_subjectscheme")]
        public CollegeRuleSettingsDTO get_subjectscheme([FromBody]  CollegeRuleSettingsDTO data)
        {
            return _intf.get_subjectscheme(data);
        }
        [Route("get_schemetype")]
        public CollegeRuleSettingsDTO get_schemetype([FromBody]  CollegeRuleSettingsDTO data)
        {
            return _intf.get_schemetype(data);
        }
        [Route("get_subjects")]
        public CollegeRuleSettingsDTO get_subjects([FromBody]  CollegeRuleSettingsDTO data)
        {
            return _intf.get_subjects(data);
        }
        [Route("saveddata")]
        public CollegeRuleSettingsDTO saveddata([FromBody]  CollegeRuleSettingsDTO data)
        {
            return _intf.saveddata(data);
        }
        [Route("getalldetailsviewrecords")]
        public CollegeRuleSettingsDTO getalldetailsviewrecords([FromBody]  CollegeRuleSettingsDTO data)
        {
            return _intf.getalldetailsviewrecords(data);
        }
        [Route("viewrecordspopup_subgrps")]
        public CollegeRuleSettingsDTO viewrecordspopup_subgrps([FromBody]  CollegeRuleSettingsDTO data)
        {
            return _intf.viewrecordspopup_subgrps(data);
        }
        [Route("getalldetailsviewrecords_sub_grp_exms")]
        public CollegeRuleSettingsDTO getalldetailsviewrecords_sub_grp_exms([FromBody]  CollegeRuleSettingsDTO data)
        {
            return _intf.getalldetailsviewrecords_sub_grp_exms(data);
        }
        [Route("editdeatils")]
        public CollegeRuleSettingsDTO editdeatils([FromBody]  CollegeRuleSettingsDTO data)
        {
            return _intf.editdeatils(data);
        }
        [Route("deactivate")]
        public CollegeRuleSettingsDTO deactivate([FromBody]  CollegeRuleSettingsDTO data)
        {
            return _intf.deactivate(data);
        }
        [Route("deactivatesubject")]
        public CollegeRuleSettingsDTO deactivatesubject([FromBody]  CollegeRuleSettingsDTO data)
        {
            return _intf.deactivatesubject(data);
        }
        [Route("deactivategroup")]
        public CollegeRuleSettingsDTO deactivategroup([FromBody]  CollegeRuleSettingsDTO data)
        {
            return _intf.deactivategroup(data);
        }
        [Route("deactivateexam")]
        public CollegeRuleSettingsDTO deactivateexam([FromBody]  CollegeRuleSettingsDTO data)
        {
            return _intf.deactivateexam(data);
        }


    }
}
