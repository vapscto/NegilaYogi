using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Common.Interface;
using PreadmissionDTOs.NAAC;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Common.Facade
{
    [Route("api/[controller]")]
    public class NAAC_User_PrivilegesFacadeController : Controller
    {
        public NAAC_User_PrivilegesInterface _interface; 

        public NAAC_User_PrivilegesFacadeController(NAAC_User_PrivilegesInterface _inter)
        {
            _interface = _inter;
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

        [Route("Getdetails")]
        public NAAC_User_PrivilegesDTO Getdetails([FromBody] NAAC_User_PrivilegesDTO data)
        {         
            return _interface.Getdetails(data);
        }

        [Route("onchangeemployee")]
        public NAAC_User_PrivilegesDTO onchangeemployee([FromBody] NAAC_User_PrivilegesDTO data)
        {
            return _interface.onchangeemployee(data);
        }

        [Route("onchangecriteria")]
        public NAAC_User_PrivilegesDTO onchangecriteria([FromBody] NAAC_User_PrivilegesDTO data)
        {
            return _interface.onchangecriteria(data);
        }

        [Route("savedata")]
        public NAAC_User_PrivilegesDTO savedata([FromBody] NAAC_User_PrivilegesDTO data)
        {
            return _interface.savedata(data);
        }
        [Route("viewrecord")]
        public NAAC_User_PrivilegesDTO viewrecord([FromBody] NAAC_User_PrivilegesDTO data)
        {
            return _interface.viewrecord(data);
        }
        [Route("deactivate")]
        public NAAC_User_PrivilegesDTO deactivate([FromBody] NAAC_User_PrivilegesDTO data)
        {
            return _interface.deactivate(data);
        }

        // Master Naac Criteria
        [Route("OnChangeInstituionType")]
        public NAAC_User_PrivilegesDTO OnChangeInstituionType([FromBody] NAAC_User_PrivilegesDTO data)
        {
            return _interface.OnChangeInstituionType(data);
        }

        [Route("SaveTab1")]
        public NAAC_User_PrivilegesDTO SaveTab1([FromBody] NAAC_User_PrivilegesDTO data)
        {
            return _interface.SaveTab1(data);
        }

        [Route("EditTab1")]
        public NAAC_User_PrivilegesDTO EditTab1([FromBody] NAAC_User_PrivilegesDTO data)
        {
            return _interface.EditTab1(data);
        }

        [Route("OnChangeCriteriaName")]
        public NAAC_User_PrivilegesDTO OnChangeCriteriaName([FromBody] NAAC_User_PrivilegesDTO data)
        {
            return _interface.OnChangeCriteriaName(data);
        }

        [Route("SaveTab2")]
        public NAAC_User_PrivilegesDTO SaveTab2([FromBody] NAAC_User_PrivilegesDTO data)
        {
            return _interface.SaveTab2(data);
        }

        [Route("EditTab2")]
        public NAAC_User_PrivilegesDTO EditTab2([FromBody] NAAC_User_PrivilegesDTO data)
        {
            return _interface.EditTab2(data);
        }

        [Route("SaveTab3")]
        public NAAC_User_PrivilegesDTO SaveTab3([FromBody] NAAC_User_PrivilegesDTO data)
        {
            return _interface.SaveTab3(data);
        }

        [Route("EditTab3")]
        public NAAC_User_PrivilegesDTO EditTab3([FromBody] NAAC_User_PrivilegesDTO data)
        {
            return _interface.EditTab3(data);
        }

        [Route("OnChangeCriteriaNameLevelOne")]
        public NAAC_User_PrivilegesDTO OnChangeCriteriaNameLevelOne([FromBody] NAAC_User_PrivilegesDTO data)
        {
            return _interface.OnChangeCriteriaNameLevelOne(data);
        }
    }

}

