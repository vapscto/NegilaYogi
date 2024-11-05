
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
//using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MasterLifeSkillFacadeController : Controller
    {
        public MasterLifeSkillInterface _MasterLifeSkill;

        public MasterLifeSkillFacadeController(MasterLifeSkillInterface MasterLifeSkill)
        {
            _MasterLifeSkill = MasterLifeSkill;
        }


        [Route("Getdetails")]
        public MasterLifeSkillDTO Getdetails([FromBody]MasterLifeSkillDTO data)//int IVRMM_Id
        {           
            return _MasterLifeSkill.Getdetails(data);           
        }

        [Route("editdetails")]
        public MasterLifeSkillDTO editdetails([FromBody]MasterLifeSkillDTO ID)
        {
            return _MasterLifeSkill.editdetails(ID);
        }
        

        [Route("savedata")]
        public MasterLifeSkillDTO savedata([FromBody] MasterLifeSkillDTO data)
        {
            return _MasterLifeSkill.savedata(data);
        }
       
        [Route("deactivate")]
        public MasterLifeSkillDTO deactivate([FromBody] MasterLifeSkillDTO data)
        {           
            return _MasterLifeSkill.deactivate(data);
        }

        //Master Life skill Area
        [Route("Savedataarea")]
        public MasterLifeSkillDTO Savedataarea([FromBody] MasterLifeSkillDTO data)
        {
            return _MasterLifeSkill.Savedataarea(data);
        }
        [Route("editdetailsarea")]
        public MasterLifeSkillDTO editdetailsarea([FromBody] MasterLifeSkillDTO data)
        {
            return _MasterLifeSkill.editdetailsarea(data);
        }
        [Route("deactivatearea")]
        public MasterLifeSkillDTO deactivatearea([FromBody] MasterLifeSkillDTO data)
        {
            return _MasterLifeSkill.deactivatearea(data);
        }
        [Route("validateordernumber")]
        public MasterLifeSkillDTO validateordernumber([FromBody] MasterLifeSkillDTO data)
        {
            return _MasterLifeSkill.validateordernumber(data);
        }

        //Master Life skill Area Mapping
        [Route("Savedataareamapping")]
        public MasterLifeSkillDTO Savedataareamapping([FromBody] MasterLifeSkillDTO data)
        {
            return _MasterLifeSkill.Savedataareamapping(data);
        }
        [Route("editdetailsareamapping")]
        public MasterLifeSkillDTO editdetailsareamapping([FromBody] MasterLifeSkillDTO data)
        {
            return _MasterLifeSkill.editdetailsareamapping(data);
        }
        [Route("deactivateareamapping")]
        public MasterLifeSkillDTO deactivateareamapping([FromBody] MasterLifeSkillDTO data)
        {
            return _MasterLifeSkill.deactivateareamapping(data);
        }
        [Route("getgrade")]
        public MasterLifeSkillDTO getgrade([FromBody] MasterLifeSkillDTO data)
        {
            return _MasterLifeSkill.getgrade(data);
        }
        

    }
}
