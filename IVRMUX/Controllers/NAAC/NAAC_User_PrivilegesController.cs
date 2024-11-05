using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC
{
    [Route("api/[controller]")]
    public class NAAC_User_PrivilegesController : Controller
    {
        public NAAC_User_PrivilegesDelegate _delg = new NAAC_User_PrivilegesDelegate();

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

        [Route("Getdetails/{id:int}")]
        public NAAC_User_PrivilegesDTO Getdetails(int id)
        {
            NAAC_User_PrivilegesDTO data = new NAAC_User_PrivilegesDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.Getdetails(data);
        }

        [Route("onchangeemployee")]
        public NAAC_User_PrivilegesDTO onchangeemployee([FromBody] NAAC_User_PrivilegesDTO data)
        {           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangeemployee(data);
        }

        [Route("onchangecriteria")]
        public NAAC_User_PrivilegesDTO onchangecriteria([FromBody] NAAC_User_PrivilegesDTO data)
        {           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangecriteria(data);
        }

        [Route("savedata")]
        public NAAC_User_PrivilegesDTO savedata([FromBody] NAAC_User_PrivilegesDTO data)
        {           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.savedata(data);
        }
        [Route("viewrecord")]
        public NAAC_User_PrivilegesDTO viewrecord([FromBody] NAAC_User_PrivilegesDTO data)
        {           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.viewrecord(data);
        }

        [Route("deactivate")]
        public NAAC_User_PrivilegesDTO deactivate([FromBody] NAAC_User_PrivilegesDTO data)
        {           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.deactivate(data);
        }

        // Master Naac Criteria
        [Route("OnChangeInstituionType")]
        public NAAC_User_PrivilegesDTO OnChangeInstituionType([FromBody] NAAC_User_PrivilegesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeInstituionType(data);
        }

        [Route("SaveTab1")]
        public NAAC_User_PrivilegesDTO SaveTab1([FromBody] NAAC_User_PrivilegesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveTab1(data);
        }

        [Route("EditTab1")]
        public NAAC_User_PrivilegesDTO EditTab1([FromBody] NAAC_User_PrivilegesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.EditTab1(data);
        }

        [Route("OnChangeCriteriaName")]
        public NAAC_User_PrivilegesDTO OnChangeCriteriaName([FromBody] NAAC_User_PrivilegesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeCriteriaName(data);
        }

        [Route("SaveTab2")]
        public NAAC_User_PrivilegesDTO SaveTab2([FromBody] NAAC_User_PrivilegesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveTab2(data);
        }

        [Route("EditTab2")]
        public NAAC_User_PrivilegesDTO EditTab2([FromBody] NAAC_User_PrivilegesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.EditTab2(data);
        }

        [Route("SaveTab3")]
        public NAAC_User_PrivilegesDTO SaveTab3([FromBody] NAAC_User_PrivilegesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveTab3(data);
        }

        [Route("EditTab3")]
        public NAAC_User_PrivilegesDTO EditTab3([FromBody] NAAC_User_PrivilegesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.EditTab3(data);
        }

        [Route("OnChangeCriteriaNameLevelOne")]
        public NAAC_User_PrivilegesDTO OnChangeCriteriaNameLevelOne([FromBody] NAAC_User_PrivilegesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeCriteriaNameLevelOne(data);
        }
    }
}
